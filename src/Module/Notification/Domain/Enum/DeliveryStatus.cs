using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain.Enum
{
    public enum DeliveryStatus 
    { 
        Pending = 1, 
        Sent = 2, 
        Failed = 3, 
        Processing = 4 
    }
}
