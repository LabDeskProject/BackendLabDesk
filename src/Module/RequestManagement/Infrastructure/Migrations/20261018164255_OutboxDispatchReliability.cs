using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace RequestManagement.Infrastructure.Migrations;
public partial class OutboxDispatchReliability : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("IF COL_LENGTH(N'request_management.OutboxMessages', N'ClaimedAtUtc') IS NULL ALTER TABLE [request_management].[OutboxMessages] ADD [ClaimedAtUtc] datetime2 NULL; IF COL_LENGTH(N'request_management.OutboxMessages', N'AttemptCount') IS NULL ALTER TABLE [request_management].[OutboxMessages] ADD [AttemptCount] int NOT NULL CONSTRAINT [DF_OutboxMessages_AttemptCount] DEFAULT(0); IF COL_LENGTH(N'request_management.OutboxMessages', N'LastError') IS NULL ALTER TABLE [request_management].[OutboxMessages] ADD [LastError] nvarchar(1000) NULL;");
        migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_OutboxMessages_ProcessedAtUtc_OccurredAtUtc' AND object_id=OBJECT_ID(N'[request_management].[OutboxMessages]')) DROP INDEX [IX_OutboxMessages_ProcessedAtUtc_OccurredAtUtc] ON [request_management].[OutboxMessages]; IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_OutboxMessages_ProcessedAtUtc_ClaimedAtUtc_OccurredAtUtc' AND object_id=OBJECT_ID(N'[request_management].[OutboxMessages]')) CREATE INDEX [IX_OutboxMessages_ProcessedAtUtc_ClaimedAtUtc_OccurredAtUtc] ON [request_management].[OutboxMessages] ([ProcessedAtUtc],[ClaimedAtUtc],[OccurredAtUtc]);");
    }
    protected override void Down(MigrationBuilder migrationBuilder) { }
}
