using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.ResolveSlaClock
{
    public sealed record ResolveSlaClockCommand(Guid ClockId, DateTime? ResolvedAtUtc = null) : IRequest;
}
