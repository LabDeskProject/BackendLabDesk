using Notification.Infrastructure.Messaging;
using System.Text.Json;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.Application;
using Notification.Application.Interfaces;
using Notification.Domain;
using Notification.Domain.Enum;
using Audit.Application;
using Audit.Application.Interfaces;
namespace Notification.Infrastructure.Dispatcher_BackgroundService;

public sealed class ModuleOutboxDispatcher(IServiceScopeFactory scopes, ILogger<ModuleOutboxDispatcher> logger, IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var rabbitEnabled = configuration.GetValue("RabbitMq:Enabled", false);
        if (!rabbitEnabled)
            logger.LogInformation("RabbitMQ outbox publishing disabled; SQL outbox events will continue through the existing notification/audit flow.");

        var initialized = false;
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopes.CreateAsyncScope();
                var worker = scope.ServiceProvider.GetRequiredService<ModuleOutboxWorker>();
                if (!rabbitEnabled)
                {
                    var legacyCount = await worker.ProcessBatchAsync(50, stoppingToken);
                    if (legacyCount == 0) await timer.WaitForNextTickAsync(stoppingToken);
                    continue;
                }
                if (!initialized)
                {
                    await scope.ServiceProvider.GetRequiredService<IOutboxMessagePublisher>().InitializeAsync(stoppingToken);
                    initialized = true;
                }
                var count = await worker.PublishBatchAsync(50, stoppingToken);
                if (count == 0) await timer.WaitForNextTickAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }
            catch (Exception ex) { logger.LogError(ex, "Module outbox polling failed"); await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); }
        }
    }
}

public sealed class ModuleOutboxWorker(NotificationDbContext db, INotificationRepository notifications, IAuditIdempotentWriter audit, ILogger<ModuleOutboxWorker> logger, IOutboxMessagePublisher publisher)
{
    private sealed record OutboxRow(Guid Id, string Type, string Payload, string SourceSchema, DateTime OccurredAtUtc);

    public async Task<int> PublishBatchAsync(int batchSize, CancellationToken ct)
    {
        var connection = db.Database.GetDbConnection();
        var opened = connection.State != System.Data.ConnectionState.Open;
        if (opened) await db.Database.OpenConnectionAsync(ct);
        try
        {
            List<OutboxRow> rows;
            await using (var transaction = await connection.BeginTransactionAsync(ct))
            {
                rows = new();
                foreach (var schema in new[] { "request_management", "workflow_approval" })
                {
                    var sql = $";WITH due AS (SELECT TOP (@take) Id FROM [{schema}].OutboxMessages WITH (UPDLOCK, READPAST, ROWLOCK) WHERE ProcessedAtUtc IS NULL AND (ClaimedAtUtc IS NULL OR ClaimedAtUtc < DATEADD(minute,-5,SYSUTCDATETIME())) ORDER BY OccurredAtUtc, Id) UPDATE o SET ClaimedAtUtc=SYSUTCDATETIME() OUTPUT inserted.Id, inserted.Type, inserted.Payload, '{schema}' AS SourceSchema, inserted.OccurredAtUtc FROM [{schema}].OutboxMessages o INNER JOIN due ON due.Id=o.Id;";
                    rows.AddRange((await connection.QueryAsync<OutboxRow>(new CommandDefinition(sql, new { take = Math.Clamp(batchSize, 1, 100) }, transaction, cancellationToken: ct))).AsList());
                }
                await transaction.CommitAsync(ct);
            }

            OutboxMetrics.Pending.Set(rows.Count);
            var published = 0;
            foreach (var row in rows)
            {
                try
                {
                    await RouteAsync(row, ct);
                    await publisher.PublishAsync(new OutboxEnvelope(row.Id, row.Type, row.SourceSchema, row.OccurredAtUtc, row.Payload), ct);
                    await connection.ExecuteAsync(new CommandDefinition($"UPDATE [{row.SourceSchema}].[OutboxMessages] SET ProcessedAtUtc=SYSUTCDATETIME(), AttemptCount=AttemptCount+1, ClaimedAtUtc=NULL, LastError=NULL WHERE Id=@id AND ProcessedAtUtc IS NULL", new { row.Id }, cancellationToken: ct));
                    OutboxMetrics.Published.WithLabels(row.Type).Inc();
                    published++;
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "Outbox processing or RabbitMQ publish failed for event {EventId} type {EventType}", row.Id, row.Type);
                    await connection.ExecuteAsync(new CommandDefinition($"UPDATE [{row.SourceSchema}].[OutboxMessages] SET AttemptCount=AttemptCount+1, ClaimedAtUtc=NULL, LastError=@error WHERE Id=@id AND ProcessedAtUtc IS NULL", new { row.Id, error = ex.Message.Length > 1000 ? ex.Message[..1000] : ex.Message }, cancellationToken: ct));
                    OutboxMetrics.PublishFailures.WithLabels(row.Type).Inc();
                }
            }
            OutboxMetrics.Pending.Set(0);
            return rows.Count;
        }
        finally { if (opened) await db.Database.CloseConnectionAsync(); }
    }

    public async Task<int> ProcessBatchAsync(int batchSize, CancellationToken ct)
    {
        var connection = db.Database.GetDbConnection();
        var opened = connection.State != System.Data.ConnectionState.Open;
        if (opened) await db.Database.OpenConnectionAsync(ct);
        try
        {
            var rows = new List<OutboxRow>();
            await using (var transaction = await connection.BeginTransactionAsync(ct))
            {
                foreach (var schema in new[] { "request_management", "workflow_approval" })
                {
                    var sql = $";WITH due AS (SELECT TOP (@take) Id FROM [{schema}].OutboxMessages WITH (UPDLOCK, READPAST, ROWLOCK) WHERE ProcessedAtUtc IS NULL AND (ClaimedAtUtc IS NULL OR ClaimedAtUtc < DATEADD(minute,-5,SYSUTCDATETIME())) ORDER BY OccurredAtUtc, Id) UPDATE o SET ClaimedAtUtc=SYSUTCDATETIME() OUTPUT inserted.Id, inserted.Type, inserted.Payload, '{schema}' AS SourceSchema, inserted.OccurredAtUtc FROM [{schema}].OutboxMessages o INNER JOIN due ON due.Id=o.Id;";
                    rows.AddRange((await connection.QueryAsync<OutboxRow>(new CommandDefinition(sql, new { take = Math.Clamp(batchSize, 1, 100) }, transaction, cancellationToken: ct))).AsList());
                }
                await transaction.CommitAsync(ct);
            }

            var handled = 0;
            foreach (var row in rows)
            {
                try
                {
                    await RouteAsync(row, ct);
                    await connection.ExecuteAsync(new CommandDefinition($"UPDATE [{row.SourceSchema}].[OutboxMessages] SET ProcessedAtUtc=SYSUTCDATETIME(), AttemptCount=AttemptCount+1, ClaimedAtUtc=NULL, LastError=NULL WHERE Id=@id AND ProcessedAtUtc IS NULL", new { row.Id }, cancellationToken: ct));
                    handled++;
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "SQL outbox routing failed for event {EventId} type {EventType}", row.Id, row.Type);
                    await connection.ExecuteAsync(new CommandDefinition($"UPDATE [{row.SourceSchema}].[OutboxMessages] SET AttemptCount=AttemptCount+1, ClaimedAtUtc=NULL, LastError=@error WHERE Id=@id AND ProcessedAtUtc IS NULL", new { row.Id, error = ex.Message.Length > 1000 ? ex.Message[..1000] : ex.Message }, cancellationToken: ct));
                }
            }
            return rows.Count;
        }
        finally { if (opened) await db.Database.CloseConnectionAsync(); }
    }

    private async Task RouteAsync(OutboxRow row, CancellationToken ct)
    {
        using var doc = JsonDocument.Parse(row.Payload); var root = doc.RootElement;
        var organizationId = GetGuid(root, "OrganizationId") ?? throw new InvalidOperationException("Outbox payload has no organization ID.");
        var entityId = GetGuid(root, "RequestId") ?? GetGuid(root, "Id") ?? throw new InvalidOperationException("Outbox payload has no request ID.");
        var at = GetDate(root, "OccurredAtUtc") ?? GetDate(root, "DecidedAtUtc") ?? DateTime.UtcNow;
        var actor = GetGuid(root, "SubmittedByUserId") ?? GetGuid(root, "ApproverUserId");
        var recipient = row.Type == "RequestSubmitted" ? GetGuid(root, "SubmittedByUserId") : GetGuid(root, "ApproverUserId");
        if (recipient is { } recipientId && recipientId != Guid.Empty)
        {
            var title = row.Type == "RequestSubmitted" ? "Request submitted" : row.Type == "ApprovalWorkflowCompleted" ? "Approval completed" : "Approval decision recorded";
            var body = row.Type == "RequestSubmitted" ? $"Request {GetString(root, "RequestNumber") ?? entityId.ToString("D")} was submitted." : $"Approval update recorded for request {entityId:D}.";
            var key = $"outbox:{row.SourceSchema}:{row.Id:D}";
            if (await notifications.FindByIdempotencyKeyAsync(organizationId, key, ct) is null)
            {
                var message = NotificationMessage.Create(organizationId, recipientId, row.Type, title, body, key, row.Payload, [DeliveryChannel.InApp], at);
                await notifications.AddAsync(message, ct); 
                await notifications.SaveChangesAsync(ct);
            }
        }
        // Append last: if inbox persistence fails, retrying the event will not duplicate its audit entry.
        await audit.AppendOnceAsync(organizationId, actor, row.Type, row.Type.StartsWith("Approval", StringComparison.Ordinal) ? "ApprovalWorkflow" : "Request", entityId.ToString("D"), $"outbox:{row.SourceSchema}:{row.Id:D}", row.Payload, at, ct);
    }
    private static Guid? GetGuid(JsonElement x, string name) => x.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.String && Guid.TryParse(p.GetString(), out var id) ? id : null;
    private static DateTime? GetDate(JsonElement x, string name) => x.TryGetProperty(name, out var p) && p.TryGetDateTime(out var value) ? value : null;
    private static string? GetString(JsonElement x, string name) => x.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.String ? p.GetString() : null;
}
