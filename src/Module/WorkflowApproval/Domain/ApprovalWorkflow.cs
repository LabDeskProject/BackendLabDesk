using WorkflowApproval.Domain.Enum;

namespace WorkflowApproval.Domain;

public sealed class ApprovalWorkflow
{
    private ApprovalWorkflow() { }
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid RequestId { get; private set; }
    public ApprovalMode Mode { get; private set; }
    public int CurrentStep { get; private set; }
    public bool IsComplete { get; private set; }
    public bool IsRejected { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public byte[] RowVersion { get; private set; } = [];
    public ICollection<ApprovalStep> Steps { get; private set; } = new List<ApprovalStep>();

    public static ApprovalWorkflow Start(Guid organizationId, Guid requestId, ApprovalMode mode, IEnumerable<IEnumerable<Guid>> approverSteps, DateTime nowUtc)
    {
        if (organizationId == Guid.Empty || requestId == Guid.Empty) 
            throw new ArgumentException("Organization and request IDs are required.");
        var normalized = approverSteps.Select(x => x.Where(id => id != Guid.Empty).Distinct().ToList()).ToList();
        if (normalized.Count == 0 || normalized.Any(x => x.Count == 0)) 
            throw new ArgumentException("Every approval step requires at least one approver.", nameof(approverSteps));
        if (mode != ApprovalMode.Sequential && normalized.Count != 1) normalized = [normalized.SelectMany(x => x).Distinct().ToList()];
        var workflow = new ApprovalWorkflow { Id = Guid.NewGuid(), OrganizationId = organizationId, RequestId = requestId, Mode = mode, CurrentStep = 1, CreatedAtUtc = nowUtc, UpdatedAtUtc = nowUtc };
        var order = 0;
        foreach (var stepUsers in normalized)
        {
            order++;
            foreach (var approver in stepUsers)
                workflow.Steps.Add(ApprovalStep.Create(workflow.Id, approver, order, nowUtc));
        }
        return workflow;
    }

    public void Decide(Guid approverUserId, ApprovalDecision decision, string? comment, DateTime nowUtc)
    {
        if (IsComplete || IsRejected) 
            throw new InvalidOperationException("Workflow is already complete.");
        if (decision is not (ApprovalDecision.Approved or ApprovalDecision.Rejected)) 
            throw new ArgumentException("Decision must be Approved or Rejected.", nameof(decision));
        var step = Steps.SingleOrDefault(x => x.ApproverUserId == approverUserId) ?? 
            throw new UnauthorizedAccessException("User is not an approver in this workflow.");
        if (step.Decision != ApprovalDecision.Pending) 
            throw new InvalidOperationException("Approver has already decided.");
        if (Mode == ApprovalMode.Sequential && step.StepOrder != CurrentStep) 
            throw new InvalidOperationException("Sequential approval must be decided in order.");
        if (Mode == ApprovalMode.AllApprovers && step.StepOrder != 1) 
            throw new InvalidOperationException("Approver is not part of the active approval step.");
        step.Record(decision, comment, nowUtc);
        if (decision == ApprovalDecision.Rejected)
        {
            IsRejected = true;
            foreach (var pending in Steps.Where(x => x.Decision == ApprovalDecision.Pending)) pending.Skip(nowUtc);
        }
        else if (Mode == ApprovalMode.AnyApprover)
        {
            IsComplete = true;
            foreach (var pending in Steps.Where(x => x.Decision == ApprovalDecision.Pending)) pending.Skip(nowUtc);
        }
        else if (Mode == ApprovalMode.AllApprovers)
        {
            IsComplete = Steps.All(x => x.Decision == ApprovalDecision.Approved);
        }
        else
        {
            var active = Steps.Where(x => x.StepOrder == CurrentStep).ToList();
            if (active.All(x => x.Decision == ApprovalDecision.Approved))
            {
                CurrentStep++;
                IsComplete = !Steps.Any(x => x.StepOrder == CurrentStep);
            }
        }
        UpdatedAtUtc = nowUtc;
    }
}

