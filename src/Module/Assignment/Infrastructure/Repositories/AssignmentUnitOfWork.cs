using Assignment.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Infrastructure.Repositories
{
    public sealed class AssignmentUnitOfWork(AssignmentDbContext db) : IAssignmentUnitOfWork
    {
        public async Task SaveChangesAsync(CancellationToken ct) => _ = await db.SaveChangesAsync(ct);
    }
}
