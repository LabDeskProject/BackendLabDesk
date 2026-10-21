using Assignment.Application.Interfaces;
using Assignment.Domain;
using Assignment.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Slices.ReassignRequest
{
    public sealed class ReassignRequestHandler(IAssignmentRepository repo, IAssignmentUnitOfWork unit, IAssignmentUserContext user) : IRequestHandler<ReassignRequestCommand, Guid>
    {
        public async Task<Guid> Handle(ReassignRequestCommand cmd, CancellationToken ct)
        {
            var old = await repo.GetActiveForRequestAsyncById(cmd.AssignmentId, ct) ?? 
                throw new KeyNotFoundException("Active assignment not found.");
            if (!await repo.AssigneeIsMemberAsync(cmd.AssigneeUserId, old.OrganizationId, ct)) 
                throw new InvalidOperationException("Assignee is not an active member of the organization.");
            if (!user.IsAdmin && !await repo.IsOrganizationAdminAsync(user.UserId, old.OrganizationId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required to reassign requests.");
            if (cmd.TeamId is { } team && !await repo.IsTeamInOrganizationAsync(team, old.OrganizationId, ct)) 
                throw new InvalidOperationException("Team does not belong to the organization.");
            old.Close(AssignmentStatus.Reassigned, DateTime.UtcNow);
            var next = AssignmentRecord.Create(old.OrganizationId, old.RequestId, cmd.AssigneeUserId, user.UserId, cmd.TeamId, cmd.Note, DateTime.UtcNow);
            await repo.AddAsync(next, ct); 
            await unit.SaveChangesAsync(ct); 
            return next.Id;
        }
    }

}
