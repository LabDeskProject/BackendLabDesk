using Audit.Application.Interfaces;
using Audit.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Slices.RecordAuditEntry
{
    public sealed class RecordAuditEntryHandler(IAuditWriter writer, IAuditAccess access, IAuditUserContext user) : IRequestHandler<RecordAuditEntryCommand, Guid>
    {
        public async Task<Guid> Handle(RecordAuditEntryCommand c, CancellationToken ct)
        {
            if (!user.IsAdmin && !await access.IsOrganizationMemberAsync(c.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("User does not belong to this organization.");
            if (c.ActorUserId is not null && c.ActorUserId != user.UserId && !user.IsAdmin) 
                throw new UnauthorizedAccessException("Clients cannot write audit entries for another actor.");
            var entry = AuditEntry.Create(c.OrganizationId, user.IsAdmin && c.ActorUserId is null ? null : user.UserId, c.Action, c.EntityType, c.EntityId, c.CorrelationId, c.DetailsJson, DateTime.UtcNow);
            await writer.AppendAsync(entry.OrganizationId, entry.ActorUserId, entry.Action, entry.EntityType, entry.EntityId, entry.CorrelationId, entry.DetailsJson, entry.OccurredAtUtc, ct); 
            return entry.Id;
        }
    }
}
