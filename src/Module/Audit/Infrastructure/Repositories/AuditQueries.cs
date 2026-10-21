using Audit.Application.Contracts;
using Audit.Application.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Infrastructure.Repositories
{
    public sealed class AuditQueries(AuditDbContext db) : IAuditQueries
    {
        public async Task<IReadOnlyList<AuditEntryItem>> ListForEntityAsync(Guid organizationId, string entityType, string entityId, int skip, int take, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open; 
            if (opened) await db.Database.OpenConnectionAsync(ct);
            try 
            { 
                return (await c.QueryAsync<AuditEntryItem>(new CommandDefinition("SELECT Id, OrganizationId, ActorUserId, Action, EntityType, EntityId, CorrelationId, DetailsJson, OccurredAtUtc FROM audit.Entries WHERE OrganizationId=@organizationId AND EntityType=@entityType AND EntityId=@entityId ORDER BY OccurredAtUtc DESC, Id DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY", new { organizationId, entityType, entityId, skip, take }, cancellationToken: ct))).AsList(); 
            }
            finally 
            { 
                if (opened) await db.Database.CloseConnectionAsync(); 
            }
        }
        public async Task<IReadOnlyList<AuditEntryItem>> ListForOrganizationAsync(Guid organizationId, int skip, int take, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open; 
            if (opened) await db.Database.OpenConnectionAsync(ct);
            try 
            { 
                return (await c.QueryAsync<AuditEntryItem>(new CommandDefinition("SELECT Id, OrganizationId, ActorUserId, Action, EntityType, EntityId, CorrelationId, DetailsJson, OccurredAtUtc FROM audit.Entries WHERE OrganizationId=@organizationId ORDER BY OccurredAtUtc DESC, Id DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY", new { organizationId, skip, take }, cancellationToken: ct))).AsList(); 
            }
            finally 
            { 
                if (opened) await db.Database.CloseConnectionAsync(); 
            }
        }
    }
}
