using RequestManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Infrastructure.Repositories
{
    public sealed class RequestUnitOfWork(RequestManagementDbContext db) : IRequestUnitOfWork
    {
        public async Task SaveChangesAsync(CancellationToken ct) => _ = await db.SaveChangesAsync(ct);
    }
}
