using Assignment.Application.Contracts;
using Assignment.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Slices.ListRequestAssignments
{
    public sealed class ListRequestAssignmentsHandler(IAssignmentRepository repo, IAssignmentUserContext user) : IRequestHandler<ListRequestAssignmentsQuery, IReadOnlyList<AssignmentListItem>>
    {
        public async Task<IReadOnlyList<AssignmentListItem>> Handle(ListRequestAssignmentsQuery q, CancellationToken ct)
        {
            var organizationId = await repo.GetRequestOrganizationAsync(q.RequestId, ct) ?? 
                throw new KeyNotFoundException("Request not found.");
            if (!user.IsAdmin && !await repo.IsOrganizationMemberAsync(user.UserId, organizationId, ct)) 
                throw new UnauthorizedAccessException("User does not belong to this organization.");
            return await repo.ListForRequestAsync(q.RequestId, ct);
        }
    }
}
