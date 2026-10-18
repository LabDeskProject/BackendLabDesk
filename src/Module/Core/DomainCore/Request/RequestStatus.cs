using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Request
{
    public enum RequestStatus
    {
        Submitted = 1,

        Assigned = 2,

        InProgress = 3,

        Completed = 4,

        Rejected = 5,

        Cancelled = 6
    }
}
