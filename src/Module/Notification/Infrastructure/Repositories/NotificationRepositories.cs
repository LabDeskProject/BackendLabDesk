using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Notification.Application.Contracts;
using Notification.Application.Interfaces;
using Notification.Domain;
using Notification.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Repositories
{
    public sealed class NotificationRepositories(NotificationDbContext db) : INotificationRepository
    {
        public Task<NotificationMessage?> FindByIdempotencyKeyAsync(Guid organizationId, string key, CancellationToken ct) => db.Notifications.Include(x => x.Deliveries).SingleOrDefaultAsync(x => x.OrganizationId == organizationId && x.IdempotencyKey == key, ct);
        public Task AddAsync(NotificationMessage message, CancellationToken ct) => db.Notifications.AddAsync(message, ct).AsTask();
        public Task<bool> IsMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [OrganizationId]={0} AND [UserId]={1} AND [RemovedAtUtc] IS NULL", organizationId, userId).AnyAsync(ct);
        public async Task<bool> MarkReadAsync(Guid id, Guid userId, DateTime atUtc, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            try 
            { 
                return await c.ExecuteAsync(new CommandDefinition("UPDATE notification.Notifications SET ReadAtUtc=@atUtc WHERE Id=@id AND RecipientUserId=@userId AND ReadAtUtc IS NULL", new { id, userId, atUtc }, cancellationToken: ct)) == 1; 
            }
            finally 
            { 
                if (opened) await db.Database.CloseConnectionAsync(); 
            }
        }
        public async Task<IReadOnlyList<NotificationItem>> ListForUserAsync(Guid org, Guid user, int skip, int take, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            try 
            { 
                return (await c.QueryAsync<NotificationItem>(new CommandDefinition("SELECT Id, OrganizationId, RecipientUserId, Type, Title, Body, PayloadJson, CreatedAtUtc, CAST(CASE WHEN ReadAtUtc IS NULL THEN 0 ELSE 1 END AS bit) AS IsRead FROM notification.Notifications WHERE OrganizationId=@org AND RecipientUserId=@user ORDER BY CreatedAtUtc DESC, Id DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY", new { org, user, skip, take }, cancellationToken: ct))).AsList(); 
            }
            finally 
            { 
                if (opened) await db.Database.CloseConnectionAsync(); 
            }
        }
        public async Task<IReadOnlyList<(NotificationDelivery Delivery, NotificationMessage Message)>> ClaimDueDeliveriesAsync(DateTime now, int take, CancellationToken ct)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(ct);
            var c = db.Database.GetDbConnection();
            var claimed = (await c.QueryAsync<ClaimedDelivery>(new CommandDefinition(";WITH due AS (SELECT TOP (@take) * FROM notification.Deliveries WITH (UPDLOCK, READPAST, ROWLOCK) WHERE Status IN (1,3,4) AND NextAttemptAtUtc <= @now AND (Status <> 4 OR ClaimedAtUtc <= DATEADD(minute,-5,@now)) ORDER BY NextAttemptAtUtc, Id) UPDATE due SET Status=4, ClaimedAtUtc=@now, NextAttemptAtUtc=DATEADD(minute,5,@now) OUTPUT inserted.Id, inserted.NotificationId", new { take, now }, transaction.GetDbTransaction(), cancellationToken: ct))).AsList();
            if (claimed.Count == 0) 
            { 
                await transaction.CommitAsync(ct); return []; 
            }

            var ids = claimed.Select(x => x.NotificationId).Distinct().ToArray();
            var messages = await db.Notifications.Include(x => x.Deliveries).Where(x => ids.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);
            var pairs = new List<(NotificationDelivery, NotificationMessage)>();
            foreach (var item in claimed)
            {
                if (!messages.TryGetValue(item.NotificationId, out var message)) 
                    continue;
                var delivery = message.Deliveries.Single(x => x.Id == item.Id);
                db.Entry(delivery).Property(x => x.Status).CurrentValue = DeliveryStatus.Processing;
                db.Entry(delivery).Property(x => x.ClaimedAtUtc).CurrentValue = now;
                db.Entry(delivery).Property(x => x.NextAttemptAtUtc).CurrentValue = now.AddMinutes(5);
                pairs.Add((delivery, message));
            }
            await transaction.CommitAsync(ct); 
            return pairs;
        }
        private sealed record ClaimedDelivery(Guid Id, Guid NotificationId);
        public async Task SaveChangesAsync(CancellationToken ct) => _ = await db.SaveChangesAsync(ct);
    }
}
