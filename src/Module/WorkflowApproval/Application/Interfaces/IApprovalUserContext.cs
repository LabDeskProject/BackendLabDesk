using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowApproval.Application.Interfaces
{
    public interface IApprovalUserContext 
    { 
        Guid UserId { get; } 
        bool IsAdmin { get; } 
    }
}
