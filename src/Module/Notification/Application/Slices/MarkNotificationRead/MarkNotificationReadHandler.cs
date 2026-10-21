using MediatR;
using Notification.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Slices.MarkNotificationRead
{
    public sealed class MarkNotificationReadHandler(INotificationRepository repo, INotificationUserContext user) : IRequestHandler<MarkNotificationReadCommand, bool>
    {
        public Task<bool> Handle(MarkNotificationReadCommand c, CancellationToken ct) => repo.MarkReadAsync(c.NotificationId, user.UserId, DateTime.UtcNow, ct);
    }
}
