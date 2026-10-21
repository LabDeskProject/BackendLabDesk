using Notification.Application;
using Notification.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Notification.Domain;
using Notification.Domain.Enum;
namespace Notification.Infrastructure.Sender;
public sealed class LoggingNotificationSender(ILogger<LoggingNotificationSender> logger) : INotificationSender
{
    public Task SendAsync(DeliveryChannel channel, NotificationMessage message, CancellationToken ct)
    {
        if (channel == DeliveryChannel.InApp) 
            return Task.CompletedTask;
        logger.LogWarning("Email delivery for notification {NotificationId} has no configured provider; delivery remains retryable", message.Id);
        throw new InvalidOperationException("No email delivery provider is configured.");
    }
}
