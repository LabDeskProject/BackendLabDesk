using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.RecordSlaResponse
{
    public sealed record RecordSlaResponseCommand(Guid ClockId, DateTime? RespondedAtUtc = null) : IRequest;
}
