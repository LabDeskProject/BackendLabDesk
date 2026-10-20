using Dapper;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OrganizationManagement.Infrastructure.Repositories
{
    public sealed class ServiceQueries(OrganizationManagementDbContext db) : IServiceQueries
    {
        public async Task<ServiceListItem?> GetDetailsAsync(Guid serviceId, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            try { return await c.QuerySingleOrDefaultAsync<ServiceListItem>(new CommandDefinition("SELECT Id,OrganizationId,ResponsibleTeamId,Name,Description,IsActive FROM organization_management.Services WHERE Id=@serviceId AND IsActive=1", new { serviceId }, cancellationToken: ct)); }
            finally { if (opened) await db.Database.CloseConnectionAsync(); }
        }

        public async Task<IReadOnlyList<ServiceListItem>> ListByOrganizationAsync(Guid organizationId, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            try { return (await c.QueryAsync<ServiceListItem>(new CommandDefinition("SELECT Id,OrganizationId,ResponsibleTeamId,Name,Description,IsActive FROM organization_management.Services WHERE OrganizationId=@organizationId AND IsActive=1 ORDER BY Name,Id", new { organizationId }, cancellationToken: ct))).AsList(); }
            finally { if (opened) await db.Database.CloseConnectionAsync(); }
        }
    }
}
