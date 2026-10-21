using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Domain;

namespace WorkflowApproval.Application.Slices.StartApprovalWorkflow
{
    public sealed class StartApprovalWorkflowHandler(IApprovalWorkflowRepository repo, IApprovalUnitOfWork unit, IApprovalUserContext user) : IRequestHandler<StartApprovalWorkflowCommand, Guid>
    {
        public async Task<Guid> Handle(StartApprovalWorkflowCommand command, CancellationToken ct)
        {
            if (!await repo.IsOrganizationAdminAsync(command.OrganizationId, user.UserId, ct) && !user.IsAdmin)
                throw new UnauthorizedAccessException("Organization admin role is required to start approval workflows.");
            if (!await repo.RequestExistsAsync(command.RequestId, command.OrganizationId, ct)) 
                throw new KeyNotFoundException("Request not found in this organization.");
            if (await repo.HasWorkflowAsync(command.RequestId, ct)) 
                throw new InvalidOperationException("Request already has an approval workflow.");

            var workflow = ApprovalWorkflow.Start(command.OrganizationId, command.RequestId, command.Mode, command.ApproverSteps, DateTime.UtcNow);
            await repo.AddAsync(workflow, ct);
            await unit.SaveChangesAsync(ct);
            return workflow.Id;
        }
    }
}
