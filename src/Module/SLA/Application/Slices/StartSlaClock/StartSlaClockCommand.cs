using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.StartSlaClock
{
    public sealed record StartSlaClockCommand(Guid OrganizationId, Guid RequestId, Guid ServiceId, DateTime? StartedAtUtc = null) : IRequest<Guid>;
}
