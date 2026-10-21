using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Domain.Enum
{
    public enum SlaClockStatus 
    { 
        Running = 1, 
        ResponseBreached = 2, 
        ResolutionBreached = 3, 
        Resolved = 4, 
        Cancelled = 5 
    }
}
