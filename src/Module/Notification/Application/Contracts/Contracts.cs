using Notification.Domain;
using Notification.Domain.Enum;
namespace Notification.Application.Contracts;
public sealed record NotificationItem(Guid Id, Guid OrganizationId, Guid RecipientUserId, string Type, string Title, string Body, string? PayloadJson, DateTime CreatedAtUtc, bool IsRead);
public sealed record NotificationDeliveryItem(Guid Id, Guid NotificationId, DeliveryChannel Channel, DeliveryStatus Status, int AttemptCount, DateTime NextAttemptAtUtc, DateTime? SentAtUtc, string? LastError);
