using Audit.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Infrastructure.Repositories
{
    public sealed class AuditAccess(AuditDbContext db) : IAuditAccess
    {
        public Task<bool> IsOrganizationMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [OrganizationId]={0} AND [UserId]={1} AND [RemovedAtUtc] IS NULL", organizationId, userId).AnyAsync(ct);
        public Task<bool> IsOrganizationAdminAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [OrganizationId]={0} AND [UserId]={1} AND [Role]=2 AND [RemovedAtUtc] IS NULL", organizationId, userId).AnyAsync(ct);
    }
}
