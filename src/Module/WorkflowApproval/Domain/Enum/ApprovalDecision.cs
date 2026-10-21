using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowApproval.Domain.Enum
{
    public enum ApprovalDecision 
    { 
        Pending = 0, 
        Approved = 1, 
        Rejected = 2, 
        Skipped = 3 
    }
}
