using MediatR;
using Notification.Application.Contracts;
using Notification.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Slices.ListMyNotifications
{
    public sealed class ListMyNotificationsHandler(INotificationRepository repo, INotificationUserContext user) : IRequestHandler<ListMyNotificationsQuery, IReadOnlyList<NotificationItem>>
    {
        public async Task<IReadOnlyList<NotificationItem>> Handle(ListMyNotificationsQuery q, CancellationToken ct)
        {
            if (!await repo.IsMemberAsync(q.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Active organization membership is required.");
            return await repo.ListForUserAsync(q.OrganizationId, user.UserId, (q.Page - 1) * q.PageSize, q.PageSize, ct);
        }
    }
}
