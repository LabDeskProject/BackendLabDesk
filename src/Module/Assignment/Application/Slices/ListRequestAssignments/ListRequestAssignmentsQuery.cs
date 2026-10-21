using Assignment.Application.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Slices.ListRequestAssignments
{
    public sealed record ListRequestAssignmentsQuery(Guid RequestId) : IRequest<IReadOnlyList<AssignmentListItem>>;
}
