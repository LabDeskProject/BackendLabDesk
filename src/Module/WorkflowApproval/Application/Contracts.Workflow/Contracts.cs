using WorkflowApproval.Domain;
using WorkflowApproval.Domain.Enum;

namespace WorkflowApproval.Application.Contracts.Workflow;

public sealed record ApprovalWorkflowItem(Guid Id, Guid OrganizationId, Guid RequestId, ApprovalMode Mode, int CurrentStep, bool IsComplete, bool IsRejected, DateTime CreatedAtUtc);
public sealed record ApprovalStepItem(Guid ApproverUserId, int StepOrder, ApprovalDecision Decision, string? Comment, DateTime? DecidedAtUtc);
public sealed record ApprovalWorkflowDetails(ApprovalWorkflowItem Workflow, IReadOnlyList<ApprovalStepItem> Steps);


