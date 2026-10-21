using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Domain.Status
{
    public enum RequestStatus 
    { 
        Submitted = 1, 
        InReview = 2, 
        Approved = 3, 
        Rejected = 4, 
        Cancelled = 5, 
        Completed = 6 
    }
}
