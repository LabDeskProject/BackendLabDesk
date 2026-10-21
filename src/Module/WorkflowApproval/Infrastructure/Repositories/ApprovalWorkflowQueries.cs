using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Application.Contracts.Workflow;
using WorkflowApproval.Application.Interfaces;

namespace WorkflowApproval.Infrastructure.Repositories
{
    public sealed class ApprovalWorkflowQueries(WorkflowApprovalDbContext db) : IApprovalWorkflowQueries
    {
        public async Task<ApprovalWorkflowDetails?> GetAsync(Guid workflowId, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            try
            {
                var w = await c.QuerySingleOrDefaultAsync<ApprovalWorkflowItem>(new CommandDefinition("SELECT Id, OrganizationId, RequestId, Mode, CurrentStep, IsComplete, IsRejected, CreatedAtUtc FROM workflow_approval.ApprovalWorkflows WHERE Id=@workflowId", new { workflowId }, cancellationToken: ct));
                if (w is null) return null;
                var steps = (await c.QueryAsync<ApprovalStepItem>(new CommandDefinition("SELECT ApproverUserId, StepOrder, Decision, Comment, DecidedAtUtc FROM workflow_approval.ApprovalSteps WHERE WorkflowId=@workflowId ORDER BY StepOrder, ApproverUserId", new { workflowId }, cancellationToken: ct))).AsList();
                return new ApprovalWorkflowDetails(w, steps);
            }
            finally { if (opened) await db.Database.CloseConnectionAsync(); }
        }
    }
}
