using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Application.Contracts.Workflow;
using WorkflowApproval.Application.Interfaces;

namespace WorkflowApproval.Application.Slices.GetApprovalWorkflow
{
    public sealed class GetApprovalWorkflowHandler(IApprovalWorkflowQueries queries, IApprovalWorkflowRepository repo, IApprovalUserContext user) : IRequestHandler<GetApprovalWorkflowQuery, ApprovalWorkflowDetails?>
    {
        public async Task<ApprovalWorkflowDetails?> Handle(GetApprovalWorkflowQuery request, CancellationToken ct)
        {
            var result = await queries.GetAsync(request.WorkflowId, ct);
            if (result is null) return null;
            if (!user.IsAdmin && !await repo.IsOrganizationMemberAsync(result.Workflow.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("User does not belong to this organization.");
            return result;
        }
    }
}
