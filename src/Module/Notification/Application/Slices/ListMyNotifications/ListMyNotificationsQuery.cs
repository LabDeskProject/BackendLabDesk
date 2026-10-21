using MediatR;
using Notification.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Slices.ListMyNotifications
{
    public sealed record ListMyNotificationsQuery(Guid OrganizationId, int Page = 1, int PageSize = 50) : IRequest<IReadOnlyList<NotificationItem>>;

}
