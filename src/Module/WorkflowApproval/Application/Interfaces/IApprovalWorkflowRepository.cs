using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Domain;

namespace WorkflowApproval.Application.Interfaces
{
    public interface IApprovalWorkflowRepository
    {
        Task<bool> RequestExistsAsync(Guid requestId, Guid organizationId, CancellationToken ct);
        Task<bool> IsOrganizationMemberAsync(Guid organizationId, Guid userId, CancellationToken ct);
        Task<bool> IsOrganizationAdminAsync(Guid organizationId, Guid userId, CancellationToken ct);
        Task<bool> HasWorkflowAsync(Guid requestId, CancellationToken ct);
        Task AddAsync(ApprovalWorkflow workflow, CancellationToken ct);
        Task<ApprovalWorkflow?> GetForUpdateAsync(Guid workflowId, CancellationToken ct);
        Task UpdateAsync(ApprovalWorkflow workflow, CancellationToken ct);
    }
}
