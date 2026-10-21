using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.CreateSlaPolicy
{
    public sealed record CreateSlaPolicyCommand(Guid OrganizationId, Guid ServiceId, int ResponseMinutes, int ResolutionMinutes) : IRequest<Guid>;

}
