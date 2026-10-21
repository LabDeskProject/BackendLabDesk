using Assignment.Application.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Slices.ListMyAssignments
{
    public sealed record ListMyAssignmentsQuery(Guid OrganizationId, int Page = 1, int PageSize = 50) : IRequest<IReadOnlyList<AssignmentListItem>>;
}
