using Audit.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Interfaces
{
    public interface IAuditQueries 
    { 
        Task<IReadOnlyList<AuditEntryItem>> ListForEntityAsync(Guid organizationId, string entityType, string entityId, int skip, int take, CancellationToken ct); Task<IReadOnlyList<AuditEntryItem>> ListForOrganizationAsync(Guid organizationId, int skip, int take, CancellationToken ct);
    }

}
