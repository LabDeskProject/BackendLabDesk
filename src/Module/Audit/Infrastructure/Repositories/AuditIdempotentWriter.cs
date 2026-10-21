using Audit.Application.Interfaces;
using Audit.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Infrastructure.Repositories
{
    public sealed class AuditIdempotentWriter(AuditDbContext db) : IAuditIdempotentWriter
    {
        public async Task AppendAsync(Guid organizationId, Guid? actorUserId, string action, string entityType, string entityId, string? correlationId, string? detailsJson, DateTime occurredAtUtc, CancellationToken ct) => await AppendCoreAsync(organizationId, actorUserId, action, entityType, entityId, correlationId, detailsJson, occurredAtUtc, ct);
        public async Task AppendOnceAsync(Guid organizationId, Guid? actorUserId, string action, string entityType, string entityId, string idempotencyKey, string? detailsJson, DateTime occurredAtUtc, CancellationToken ct) => await AppendCoreAsync(organizationId, actorUserId, action, entityType, entityId, idempotencyKey, detailsJson, occurredAtUtc, ct);
        private async Task AppendCoreAsync(Guid organizationId, Guid? actorUserId, string action, string entityType, string entityId, string? correlationId, string? detailsJson, DateTime occurredAtUtc, CancellationToken ct)
        {
            var entry = AuditEntry.Create(organizationId, actorUserId, action, entityType, entityId, correlationId, detailsJson, occurredAtUtc);
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open;
            if (opened) await db.Database.OpenConnectionAsync(ct);
            try
            {
                await using var cmd = c.CreateCommand();
                if (!string.IsNullOrWhiteSpace(correlationId))
                {
                    cmd.CommandText = "IF NOT EXISTS (SELECT 1 FROM [audit].[Entries] WITH (UPDLOCK,HOLDLOCK) WHERE [CorrelationId]=@correlation) INSERT INTO [audit].[Entries] ([Id],[OrganizationId],[ActorUserId],[Action],[EntityType],[EntityId],[CorrelationId],[DetailsJson],[OccurredAtUtc]) VALUES (@id,@org,@actor,@action,@entityType,@entityId,@correlation,@details,@at)";
                }
                else cmd.CommandText = "INSERT INTO [audit].[Entries] ([Id],[OrganizationId],[ActorUserId],[Action],[EntityType],[EntityId],[CorrelationId],[DetailsJson],[OccurredAtUtc]) VALUES (@id,@org,@actor,@action,@entityType,@entityId,@correlation,@details,@at)";
                Add(cmd, "@id", entry.Id); Add(cmd, "@org", entry.OrganizationId); Add(cmd, "@actor", entry.ActorUserId); Add(cmd, "@action", entry.Action); Add(cmd, "@entityType", entry.EntityType); Add(cmd, "@entityId", entry.EntityId); Add(cmd, "@correlation", entry.CorrelationId); Add(cmd, "@details", entry.DetailsJson); Add(cmd, "@at", entry.OccurredAtUtc);
                await cmd.ExecuteNonQueryAsync(ct);
            }
            finally { if (opened) await db.Database.CloseConnectionAsync(); }
        }

        private static void Add(System.Data.Common.DbCommand cmd, string name, object? value) 
        { 
            var p = cmd.CreateParameter(); p.ParameterName = name; p.Value = value ?? DBNull.Value; cmd.Parameters.Add(p); 
        }

    }
}
