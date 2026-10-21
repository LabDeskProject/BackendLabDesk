using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace WorkflowApproval.Infrastructure.Migrations;
public partial class OutboxDispatchReliability : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("IF COL_LENGTH(N'workflow_approval.OutboxMessages', N'ClaimedAtUtc') IS NULL ALTER TABLE [workflow_approval].[OutboxMessages] ADD [ClaimedAtUtc] datetime2 NULL; IF COL_LENGTH(N'workflow_approval.OutboxMessages', N'AttemptCount') IS NULL ALTER TABLE [workflow_approval].[OutboxMessages] ADD [AttemptCount] int NOT NULL CONSTRAINT [DF_WorkflowOutbox_AttemptCount] DEFAULT(0); IF COL_LENGTH(N'workflow_approval.OutboxMessages', N'LastError') IS NULL ALTER TABLE [workflow_approval].[OutboxMessages] ADD [LastError] nvarchar(1000) NULL;");
        migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_OutboxMessages_ProcessedAtUtc_OccurredAtUtc' AND object_id=OBJECT_ID(N'[workflow_approval].[OutboxMessages]')) DROP INDEX [IX_OutboxMessages_ProcessedAtUtc_OccurredAtUtc] ON [workflow_approval].[OutboxMessages]; IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_OutboxMessages_ProcessedAtUtc_ClaimedAtUtc_OccurredAtUtc' AND object_id=OBJECT_ID(N'[workflow_approval].[OutboxMessages]')) CREATE INDEX [IX_OutboxMessages_ProcessedAtUtc_ClaimedAtUtc_OccurredAtUtc] ON [workflow_approval].[OutboxMessages] ([ProcessedAtUtc],[ClaimedAtUtc],[OccurredAtUtc]);");
    }
    protected override void Down(MigrationBuilder migrationBuilder) { }
}
