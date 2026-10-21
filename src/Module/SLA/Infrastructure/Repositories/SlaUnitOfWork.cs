using SLA.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Infrastructure.Repositories
{
    public sealed class SlaUnitOfWork(SlaDbContext db) : ISlaUnitOfWork
    {
        public async Task SaveChangesAsync(CancellationToken ct) => _ = await db.SaveChangesAsync(ct);
    }
}
