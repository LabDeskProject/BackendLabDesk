using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.ArchiveSlaPolicy
{
    public sealed record ArchiveSlaPolicyCommand(Guid PolicyId) : IRequest;
}
