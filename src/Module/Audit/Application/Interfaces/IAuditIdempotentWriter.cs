using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Interfaces
{
    public interface IAuditIdempotentWriter : IAuditWriter 
    { 
        Task AppendOnceAsync(Guid organizationId, Guid? actorUserId, string action, string entityType, string entityId, string idempotencyKey, string? detailsJson, DateTime occurredAtUtc, CancellationToken ct); 
    }

}
