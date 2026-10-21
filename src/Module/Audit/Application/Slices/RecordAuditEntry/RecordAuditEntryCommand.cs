using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Slices.RecordAuditEntry
{
    public sealed record RecordAuditEntryCommand(Guid OrganizationId, Guid? ActorUserId, string Action, string EntityType, string EntityId, string? CorrelationId, string? DetailsJson) : IRequest<Guid>;

}
