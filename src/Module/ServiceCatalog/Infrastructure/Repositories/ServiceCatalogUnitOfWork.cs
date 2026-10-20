using ServiceCatalog.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Infrastructure.Repositories
{
    public sealed class ServiceCatalogUnitOfWork(ServiceCatalogDbContext db) : IServiceCatalogUnitOfWork
    {
        public async Task SaveChangesAsync(CancellationToken ct) => _ = await db.SaveChangesAsync(ct);
    }
}
