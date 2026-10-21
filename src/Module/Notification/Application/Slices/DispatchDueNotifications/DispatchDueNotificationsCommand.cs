using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Slices.DispatchDueNotifications
{
    public sealed record DispatchDueNotificationsCommand(int BatchSize = 100) : IRequest<int>;
}
