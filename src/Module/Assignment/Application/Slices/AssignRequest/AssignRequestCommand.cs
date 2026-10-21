using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Slices.AssignRequest
{
    public sealed record AssignRequestCommand(Guid OrganizationId, Guid RequestId, Guid AssigneeUserId, Guid? TeamId, string? Note) : IRequest<Guid>;
}
