using WorkflowApproval.Domain;
using WorkflowApproval.Domain.Enum;

namespace WorkflowApproval.Domain.Tests;

public sealed class ApprovalWorkflowTests
{
    private static readonly DateTime Now = new(2026, 5, 1, 10, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Any_approver_completes_when_one_approves()
    {
        var a = Guid.NewGuid(); var b = Guid.NewGuid();
        var workflow = ApprovalWorkflow.Start(Guid.NewGuid(), Guid.NewGuid(), ApprovalMode.AnyApprover, [new[] { a, b }], Now);
        workflow.Decide(a, ApprovalDecision.Approved, "ok", Now.AddMinutes(1));
        Assert.True(workflow.IsComplete);
        Assert.Contains(workflow.Steps, x => x.ApproverUserId == b && x.Decision == ApprovalDecision.Skipped);
    }

    [Fact]
    public void All_approver_completes_only_after_every_approval()
    {
        var a = Guid.NewGuid(); var b = Guid.NewGuid();
        var workflow = ApprovalWorkflow.Start(Guid.NewGuid(), Guid.NewGuid(), ApprovalMode.AllApprovers, [new[] { a, b }], Now);
        workflow.Decide(a, ApprovalDecision.Approved, null, Now.AddMinutes(1));
        Assert.False(workflow.IsComplete);
        workflow.Decide(b, ApprovalDecision.Approved, null, Now.AddMinutes(2));
        Assert.True(workflow.IsComplete);
    }

    [Fact]
    public void Sequential_workflow_enforces_order_then_advances()
    {
        var a = Guid.NewGuid(); var b = Guid.NewGuid();
        var workflow = ApprovalWorkflow.Start(Guid.NewGuid(), Guid.NewGuid(), ApprovalMode.Sequential, [new[] { a }, new[] { b }], Now);
        Assert.Throws<InvalidOperationException>(() => workflow.Decide(b, ApprovalDecision.Approved, null, Now.AddMinutes(1)));
        workflow.Decide(a, ApprovalDecision.Approved, null, Now.AddMinutes(2));
        Assert.Equal(2, workflow.CurrentStep);
        workflow.Decide(b, ApprovalDecision.Approved, null, Now.AddMinutes(3));
        Assert.True(workflow.IsComplete);
    }

    [Fact]
    public void Rejection_terminates_and_skips_remaining_approvers()
    {
        var a = Guid.NewGuid(); var b = Guid.NewGuid();
        var workflow = ApprovalWorkflow.Start(Guid.NewGuid(), Guid.NewGuid(), ApprovalMode.AllApprovers, [new[] { a, b }], Now);
        workflow.Decide(a, ApprovalDecision.Rejected, "no", Now.AddMinutes(1));
        Assert.True(workflow.IsRejected);
        Assert.Contains(workflow.Steps, x => x.ApproverUserId == b && x.Decision == ApprovalDecision.Skipped);
    }
}
