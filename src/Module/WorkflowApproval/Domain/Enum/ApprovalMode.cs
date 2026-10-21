using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowApproval.Domain.Enum
{
    public enum ApprovalMode 
    { 
        AnyApprover = 1, 
        AllApprovers = 2, 
        Sequential = 3 
    }
}
