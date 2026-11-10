namespace Notification.Infrastructure.Messaging;

/// <summary>Fails publication explicitly when RabbitMQ is not enabled.</summary>
public sealed class DisabledOutboxMessagePublisher : IOutboxMessagePublisher
{
    public Task InitializeAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task PublishAsync(OutboxEnvelope envelope, CancellationToken cancellationToken) =>
        throw new InvalidOperationException("RabbitMQ publishing is disabled. Set RabbitMq:Enabled=true and configure broker credentials.");
}
