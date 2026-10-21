using Assignment.Application.Contracts;
using Assignment.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Slices.ListMyAssignments
{
    public sealed class ListMyAssignmentsHandler(IAssignmentRepository repo, IAssignmentUserContext user) : IRequestHandler<ListMyAssignmentsQuery, IReadOnlyList<AssignmentListItem>>
    {
        public async Task<IReadOnlyList<AssignmentListItem>> Handle(ListMyAssignmentsQuery q, CancellationToken ct)
        {
            if (!user.IsAdmin && !await repo.IsOrganizationMemberAsync(user.UserId, q.OrganizationId, ct)) 
                throw new UnauthorizedAccessException("User does not belong to this organization.");
            return await repo.ListForUserAsync(q.OrganizationId, user.UserId, (q.Page - 1) * q.PageSize, q.PageSize, ct);
        }
    }
}
