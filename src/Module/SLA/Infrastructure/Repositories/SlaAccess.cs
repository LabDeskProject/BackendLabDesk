using Microsoft.EntityFrameworkCore;
using SLA.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Infrastructure.Repositories
{
    public sealed class SlaAccess(SlaDbContext db) : ISlaAccess
    {
        public Task<bool> IsMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [OrganizationId]={0} AND [UserId]={1} AND [RemovedAtUtc] IS NULL", organizationId, userId).AnyAsync(ct);
        public Task<bool> IsAdminAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [OrganizationId]={0} AND [UserId]={1} AND [Role]=2 AND [RemovedAtUtc] IS NULL", organizationId, userId).AnyAsync(ct);
    }
}
