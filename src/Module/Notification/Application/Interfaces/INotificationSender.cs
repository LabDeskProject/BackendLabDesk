using Notification.Domain;
using Notification.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Interfaces
{
    public interface INotificationSender 
    { 
        Task SendAsync(DeliveryChannel channel, NotificationMessage message, CancellationToken ct); 
    }

}
