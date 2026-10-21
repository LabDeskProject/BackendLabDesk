using MediatR;
using SLA.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.ListSlaBreaches
{
    public sealed record ListSlaBreachesQuery(Guid OrganizationId, int Page = 1, int PageSize = 50) : IRequest<IReadOnlyList<SlaClockItem>>;
}
