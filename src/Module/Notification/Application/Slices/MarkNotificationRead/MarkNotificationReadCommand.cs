using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Slices.MarkNotificationRead
{
    public sealed record MarkNotificationReadCommand(Guid NotificationId) : IRequest<bool>;
}
