using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Application.Contracts.Workflow;

namespace WorkflowApproval.Application.Interfaces
{
    public interface IApprovalWorkflowQueries 
    { 
        Task<ApprovalWorkflowDetails?> GetAsync(Guid workflowId, CancellationToken ct); 
    }
}
