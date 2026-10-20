using Dapper;
using Microsoft.EntityFrameworkCore;
using ServiceCatalog.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Infrastructure.Repositories
{
    public sealed class ServiceCatalogQueries(ServiceCatalogDbContext db) : IServiceCatalogQueries
    {
        public async Task<ServiceCatalogItem?> GetAsync(Guid id, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open;
            if (opened) await db.Database.OpenConnectionAsync(ct);
            try { return await c.QuerySingleOrDefaultAsync<ServiceCatalogItem>(new CommandDefinition("SELECT Id, OrganizationId, Name, Description, IsArchived FROM service_catalog.Services WHERE Id = @id", new { id }, cancellationToken: ct)); }
            finally { if (opened) await db.Database.CloseConnectionAsync(); }
        }
        public async Task<IReadOnlyList<ServiceCatalogItem>> ListAsync(Guid organizationId, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open;
            if (opened) await db.Database.OpenConnectionAsync(ct);
            try { return (await c.QueryAsync<ServiceCatalogItem>(new CommandDefinition("SELECT Id, OrganizationId, Name, Description, IsArchived FROM service_catalog.Services WHERE OrganizationId = @organizationId AND IsArchived = 0 ORDER BY Name, Id", new { organizationId }, cancellationToken: ct))).AsList(); }
            finally { if (opened) await db.Database.CloseConnectionAsync(); }
        }
    }
}
