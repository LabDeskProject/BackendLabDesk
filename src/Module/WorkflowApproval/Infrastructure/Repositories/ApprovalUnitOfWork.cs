using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Application.Interfaces;

namespace WorkflowApproval.Infrastructure.Repositories
{
    public sealed class ApprovalUnitOfWork(WorkflowApprovalDbContext db) : IApprovalUnitOfWork
    {
        public async Task SaveChangesAsync(CancellationToken ct) => _ = await db.SaveChangesAsync(ct);
    }
}
