using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Notification.Infrastructure.Messaging.RabbitMQ;

public sealed class RabbitMqOutboxMessagePublisher : IOutboxMessagePublisher, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private readonly string _exchange;
    private readonly string _queue;
    private readonly string _deadLetterExchange;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqOutboxMessagePublisher(IConfiguration configuration)
    {
        var section = configuration.GetSection("RabbitMq");
        _factory = new ConnectionFactory
        {
            HostName = section["Host"] ?? "rabbitmq",
            Port = section.GetValue("Port", 5672),
            UserName = section["User"] ?? "labdesk",
            Password = section["Password"] ?? throw new InvalidOperationException("RabbitMq:Password is required when RabbitMq:Enabled is true."),
            VirtualHost = section["VirtualHost"] ?? "/labdesk",
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
            ClientProvidedName = "labdesk-api-outbox-publisher"
        };
        _exchange = section["Exchange"] ?? "labdesk.events";
        _queue = section["Queue"] ?? "labdesk.events";
        _deadLetterExchange = section["DeadLetterExchange"] ?? "labdesk.events.dlx";
        _gate = new SemaphoreSlim(1, 1);
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken);
        try 
        { 
            await EnsureChannelAsync(cancellationToken); 
        }
        finally 
        { 
            _gate.Release(); 
        }
    }

    public async Task PublishAsync(OutboxEnvelope envelope, CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            await EnsureChannelAsync(cancellationToken);
            var channel = _channel!;
            var body = JsonSerializer.SerializeToUtf8Bytes(envelope);
            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                MessageId = envelope.EventId.ToString("D"),
                Type = envelope.EventType,
                Timestamp = new AmqpTimestamp(new DateTimeOffset(envelope.OccurredAtUtc).ToUnixTimeSeconds()),
                Headers = new Dictionary<string, object?>
                {
                    ["source-schema"] = envelope.SourceSchema,
                    ["event-version"] = 1
                }
            };

            await channel.BasicPublishAsync(_exchange, envelope.EventType, mandatory: true, properties, body, cancellationToken);
        }
        catch
        {
            await ResetChannelAsync();
            throw;
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task EnsureChannelAsync(CancellationToken cancellationToken) //Exchang and queue
    {
        if (_channel is { IsOpen: true }) 
            return;

        await ResetChannelAsync();
        _connection = await _factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(new CreateChannelOptions(publisherConfirmationsEnabled: true, publisherConfirmationTrackingEnabled: true), cancellationToken);
        await _channel.ExchangeDeclareAsync(_exchange, ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: cancellationToken);
        await _channel.ExchangeDeclareAsync(_deadLetterExchange, ExchangeType.Fanout, durable: true, autoDelete: false, cancellationToken: cancellationToken);
        var args = new Dictionary<string, object?>
        {
            ["x-queue-type"] = "quorum",
            ["x-dead-letter-exchange"] = _deadLetterExchange
        };
        await _channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false, arguments: args, cancellationToken: cancellationToken);
        await _channel.QueueBindAsync(_queue, _exchange, "#", cancellationToken: cancellationToken);
        await _channel.QueueDeclareAsync("labdesk.events.dead", durable: true, exclusive: false, autoDelete: false,
            arguments: new Dictionary<string, object?> { ["x-queue-type"] = "quorum" }, cancellationToken: cancellationToken);
        await _channel.QueueBindAsync("labdesk.events.dead", _deadLetterExchange, "", cancellationToken: cancellationToken);
    }

    private async Task ResetChannelAsync()
    {
        if (_channel is not null)
        {
            try { await _channel.DisposeAsync(); } catch { }
            _channel = null;
        }
        if (_connection is not null)
        {
            try { await _connection.DisposeAsync(); } catch { }
            _connection = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _gate.WaitAsync();
        try { await ResetChannelAsync(); }
        finally { _gate.Release(); _gate.Dispose(); }
    }
}
