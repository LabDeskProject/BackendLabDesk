using Assignment.Application.Interfaces;
using Assignment.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Slices.AssignRequest
{
    public sealed class AssignRequestHandler(IAssignmentRepository repo, IAssignmentUnitOfWork unit, IAssignmentUserContext user) : IRequestHandler<AssignRequestCommand, Guid>
    {
        public async Task<Guid> Handle(AssignRequestCommand cmd, CancellationToken ct)
        {
            if (!await repo.RequestExistsAsync(cmd.RequestId, cmd.OrganizationId, ct)) 
                throw new KeyNotFoundException("Request not found in organization.");
            if (!user.IsAdmin && !await repo.IsOrganizationAdminAsync(user.UserId, cmd.OrganizationId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required to assign requests.");
            if (!await repo.AssigneeIsMemberAsync(cmd.AssigneeUserId, cmd.OrganizationId, ct)) 
                throw new InvalidOperationException("Assignee is not an active member of the organization.");
            if (cmd.TeamId is { } team && !await repo.IsTeamInOrganizationAsync(team, cmd.OrganizationId, ct)) 
                throw new InvalidOperationException("Team does not belong to the organization.");
            var current = await repo.GetActiveForRequestAsync(cmd.RequestId, ct);
            if (current is not null) throw new InvalidOperationException("Request already has an active assignment; use reassignment.");
            var assignment = AssignmentRecord.Create(cmd.OrganizationId, cmd.RequestId, cmd.AssigneeUserId, user.UserId, cmd.TeamId, cmd.Note, DateTime.UtcNow);
            await repo.AddAsync(assignment, ct); 
            await unit.SaveChangesAsync(ct); 
            return assignment.Id;
        }
    }
}
