using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Application.Interfaces;

namespace WorkflowApproval.Application.Slices.DecideApproval
{
    public sealed class DecideApprovalHandler(IApprovalWorkflowRepository repo, IApprovalUnitOfWork unit, IApprovalUserContext user) : IRequestHandler<DecideApprovalCommand>
    {
        public async Task Handle(DecideApprovalCommand command, CancellationToken ct)
        {
            var workflow = await repo.GetForUpdateAsync(command.WorkflowId, ct) ?? 
                throw new KeyNotFoundException("Approval workflow not found.");
            if (!user.IsAdmin && !await repo.IsOrganizationMemberAsync(workflow.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("User does not belong to this organization.");
            workflow.Decide(user.UserId, command.Decision, command.Comment, DateTime.UtcNow);
            await repo.UpdateAsync(workflow, ct);
            await unit.SaveChangesAsync(ct);
        }
    }

}
