using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.UpdateSlaPolicy
{
    public sealed record UpdateSlaPolicyCommand(Guid PolicyId, int ResponseMinutes, int ResolutionMinutes) : IRequest;
}
