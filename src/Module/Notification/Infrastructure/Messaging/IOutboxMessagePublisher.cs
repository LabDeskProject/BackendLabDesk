namespace Notification.Infrastructure.Messaging;
public interface IOutboxMessagePublisher
{
    Task InitializeAsync(CancellationToken cancellationToken);
    Task PublishAsync(OutboxEnvelope envelope, CancellationToken cancellationToken);
}
