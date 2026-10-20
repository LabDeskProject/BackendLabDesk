using ServiceCatalog.Application;
using ServiceCatalog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ServiceCatalog.Infrastructure.Repositories
{
    public sealed class ServiceCatalogRepository(ServiceCatalogDbContext db) : IServiceCatalogRepository
    {
        public Task AddAsync(ServiceDefinition service, CancellationToken ct) => db.Services.AddAsync(service, ct).AsTask();

        public Task<ServiceDefinition?> GetByIdAsync(Guid serviceId, CancellationToken ct) => db.Services.SingleOrDefaultAsync(x => x.Id == serviceId, ct);

        public Task<bool> NameExistsAsync(Guid organizationId, string name, Guid? excludeId, CancellationToken ct) => db.Services.AnyAsync(x => x.OrganizationId == organizationId && x.Name == name && !x.IsArchived && (!excludeId.HasValue || x.Id != excludeId.Value), ct);

        public Task<bool> OrganizationExistsAsync(Guid organizationId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [Id] AS [Value] FROM [organization_management].[Organizations] WHERE [Id] = {0} AND [IsArchived] = 0", organizationId).AnyAsync(ct);

        public Task UpdateAsync(ServiceDefinition service, CancellationToken ct)
        {
            db.Services.Update(service);
            return Task.CompletedTask;
        }
    }
}
