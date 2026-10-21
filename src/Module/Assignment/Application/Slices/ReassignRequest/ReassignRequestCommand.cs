using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Slices.ReassignRequest
{
    public sealed record ReassignRequestCommand(Guid AssignmentId, Guid AssigneeUserId, Guid? TeamId, string? Note) : IRequest<Guid>;
}
