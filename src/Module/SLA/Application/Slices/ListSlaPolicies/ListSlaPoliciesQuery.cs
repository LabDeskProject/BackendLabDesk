using MediatR;
using SLA.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.ListSlaPolicies
{
    public sealed record ListSlaPoliciesQuery(Guid OrganizationId) : IRequest<IReadOnlyList<SlaPolicyItem>>;
}
