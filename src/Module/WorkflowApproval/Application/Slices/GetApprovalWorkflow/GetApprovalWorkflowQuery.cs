using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Application.Contracts.Workflow;

namespace WorkflowApproval.Application.Slices.GetApprovalWorkflow
{
    public sealed record GetApprovalWorkflowQuery(Guid WorkflowId) : IRequest<ApprovalWorkflowDetails?>;
}
