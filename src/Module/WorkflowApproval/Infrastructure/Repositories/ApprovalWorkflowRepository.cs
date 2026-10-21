using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Domain;

namespace WorkflowApproval.Infrastructure.Repositories
{
    public sealed class ApprovalWorkflowRepository(WorkflowApprovalDbContext db) : IApprovalWorkflowRepository
    {
        public Task AddAsync(ApprovalWorkflow workflow, CancellationToken ct) => db.Workflows.AddAsync(workflow, ct).AsTask();

        public Task<ApprovalWorkflow?> GetForUpdateAsync(Guid id, CancellationToken ct) => db.Workflows.Include(x => x.Steps).SingleOrDefaultAsync(x => x.Id == id, ct);

        public Task<bool> HasWorkflowAsync(Guid requestId, CancellationToken ct) => db.Workflows.AnyAsync(x => x.RequestId == requestId, ct);

        public Task<bool> IsOrganizationMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [OrganizationId]={0} AND [UserId]={1} AND [RemovedAtUtc] IS NULL", organizationId, userId).AnyAsync(ct);
        public Task<bool> IsOrganizationAdminAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [OrganizationId]={0} AND [UserId]={1} AND [Role]=2 AND [RemovedAtUtc] IS NULL", organizationId, userId).AnyAsync(ct);

        public Task<bool> RequestExistsAsync(Guid requestId, Guid organizationId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [Id] AS [Value] FROM [request_management].[Requests] WHERE [Id]={0} AND [OrganizationId]={1}", requestId, organizationId).AnyAsync(ct);

        public async Task UpdateAsync(ApprovalWorkflow workflow, CancellationToken ct)
        {
            db.Workflows.Update(workflow);
            var decision = workflow.Steps.OrderByDescending(x => x.DecidedAtUtc).FirstOrDefault(x => x.DecidedAtUtc is not null);
            if (decision is null) return;
            var payload = JsonSerializer.Serialize(new { workflow.Id, workflow.OrganizationId, workflow.RequestId, decision.ApproverUserId, decision.Decision, decision.Comment, decision.DecidedAtUtc, workflow.IsComplete, workflow.IsRejected });
            await db.OutboxMessages.AddAsync(ApprovalOutboxMessage.Create("ApprovalDecisionRecorded", payload, decision.DecidedAtUtc!.Value), ct);
            if (workflow.IsComplete || workflow.IsRejected)
            {
                var status = workflow.IsRejected ? "Rejected" : "Approved";
                var terminalPayload = JsonSerializer.Serialize(new { workflow.Id, workflow.OrganizationId, workflow.RequestId, decision.ApproverUserId, Status = status, OccurredAtUtc = decision.DecidedAtUtc });
                await db.OutboxMessages.AddAsync(ApprovalOutboxMessage.Create("ApprovalWorkflowCompleted", terminalPayload, decision.DecidedAtUtc.Value), ct);
            }
        }
    }
}
