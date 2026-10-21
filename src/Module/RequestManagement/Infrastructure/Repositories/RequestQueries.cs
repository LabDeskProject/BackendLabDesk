using Dapper;
using Microsoft.EntityFrameworkCore;
using RequestManagement.Application.Contracts;
using RequestManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Infrastructure.Repositories
{
    public sealed class RequestQueries(RequestManagementDbContext db) : IRequestQueries
    {
        public async Task<RequestListItem?> GetAsync(Guid id, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            try 
            { 
                return await c.QuerySingleOrDefaultAsync<RequestListItem>(new CommandDefinition("SELECT Id,OrganizationId,ServiceId,SubmittedByUserId,RequestNumber,Title,Description,FormDataJson,Status,CreatedAtUtc,UpdatedAtUtc FROM request_management.Requests WHERE Id=@id", new { id }, cancellationToken: ct));
            }
            finally 
            { 
                if (opened) await db.Database.CloseConnectionAsync();
            }
        }
        public async Task<IReadOnlyList<RequestListItem>> ListAsync(Guid organizationId, Guid userId, bool isAdmin, int skip, int take, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            try 
            { 
                return (await c.QueryAsync<RequestListItem>(new CommandDefinition("SELECT Id,OrganizationId,ServiceId,SubmittedByUserId,RequestNumber,Title,Description,FormDataJson,Status,CreatedAtUtc,UpdatedAtUtc FROM request_management.Requests WHERE OrganizationId=@organizationId AND (@isAdmin=1 OR SubmittedByUserId=@userId) ORDER BY CreatedAtUtc DESC,Id OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY", new { organizationId, userId, isAdmin, skip, take }, cancellationToken: ct))).AsList(); 
            }
            finally 
            { 
                if (opened) await db.Database.CloseConnectionAsync(); 
            }
        }
    }
}
