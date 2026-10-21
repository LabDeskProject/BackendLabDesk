using Microsoft.EntityFrameworkCore;
using RequestManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Infrastructure.Repositories
{
    public sealed class RequestSubmissionTransaction(RequestManagementDbContext db) : IRequestSubmissionTransaction
    {
        public async Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken ct)
        {
            await using var tx = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct);
            try 
            { 
                await action(ct); await tx.CommitAsync(ct); 
            } 
            catch 
            { 
                await tx.RollbackAsync(ct); throw; 
            }
        }
    }
}
