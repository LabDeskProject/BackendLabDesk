using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RequestManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Infrastructure.Repositories
{
    public sealed class RequestNumberAllocator(RequestManagementDbContext db) : IRequestNumberAllocator
    {
        public async Task<string> NextAsync(Guid organizationId, CancellationToken ct)
        {
            var connection = db.Database.GetDbConnection(); var opened = connection.State != System.Data.ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            var ownTransaction = db.Database.CurrentTransaction is null; var transaction = db.Database.CurrentTransaction?.GetDbTransaction(); System.Data.Common.DbTransaction? local = null;
            try
            {
                if (ownTransaction) local = await connection.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct); transaction ??= local;
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var sequence = await connection.QuerySingleOrDefaultAsync<long?>(new CommandDefinition("SELECT NextValue FROM request_management.RequestNumberSequences WITH (UPDLOCK,HOLDLOCK) WHERE OrganizationId=@organizationId", new { organizationId }, transaction, cancellationToken: ct));
                long allocated;
                if (sequence is null) { allocated = 1; await connection.ExecuteAsync(new CommandDefinition("INSERT INTO request_management.RequestNumberSequences (OrganizationId,NextValue) VALUES (@organizationId,2)", new { organizationId }, transaction, cancellationToken: ct)); }
                else { allocated = sequence.Value; await connection.ExecuteAsync(new CommandDefinition("UPDATE request_management.RequestNumberSequences SET NextValue=@next WHERE OrganizationId=@organizationId", new { next = allocated + 1, organizationId }, transaction, cancellationToken: ct)); }
                if (ownTransaction) await local!.CommitAsync(ct);
                return $"REQ-{organizationId.ToString("N")[..8].ToUpperInvariant()}-{today:yyyyMMdd}-{allocated:D6}";
            }
            catch { if (ownTransaction && local is not null) await local.RollbackAsync(ct); throw; }
            finally { if (local is not null) await local.DisposeAsync(); if (opened) await db.Database.CloseConnectionAsync(); }
        }
    }
}
