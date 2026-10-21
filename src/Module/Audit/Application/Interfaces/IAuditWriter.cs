using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Interfaces
{
    public interface IAuditWriter 
    { 
        Task AppendAsync(Guid organizationId, Guid? actorUserId, string action, string entityType, string entityId, string? correlationId, string? detailsJson, DateTime occurredAtUtc, CancellationToken ct);
    }
}
