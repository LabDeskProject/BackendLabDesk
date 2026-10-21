using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Domain.Enum;

namespace WorkflowApproval.Application.Slices.StartApprovalWorkflow
{
    public sealed record StartApprovalWorkflowCommand(Guid OrganizationId, Guid RequestId, ApprovalMode Mode, IReadOnlyList<IReadOnlyList<Guid>> ApproverSteps) : IRequest<Guid>;
}
