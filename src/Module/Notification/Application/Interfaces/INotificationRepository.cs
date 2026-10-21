using Notification.Application.Contracts;
using Notification.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<NotificationMessage?> FindByIdempotencyKeyAsync(Guid organizationId, string key, CancellationToken ct);
        Task AddAsync(NotificationMessage message, CancellationToken ct);
        Task<bool> MarkReadAsync(Guid id, Guid recipientUserId, DateTime atUtc, CancellationToken ct);
        Task<bool> IsMemberAsync(Guid organizationId, Guid userId, CancellationToken ct);
        Task<IReadOnlyList<(NotificationDelivery Delivery, NotificationMessage Message)>> ClaimDueDeliveriesAsync(DateTime nowUtc, int take, CancellationToken ct);
        Task<IReadOnlyList<NotificationItem>> ListForUserAsync(Guid organizationId, Guid userId, int skip, int take, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
