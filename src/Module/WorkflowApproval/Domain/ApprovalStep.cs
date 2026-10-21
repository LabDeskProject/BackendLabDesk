using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Domain.Enum;

namespace WorkflowApproval.Domain
{
    public sealed class ApprovalStep
    {
        private ApprovalStep() { }
        public Guid Id { get; private set; }
        public Guid WorkflowId { get; private set; }
        public Guid ApproverUserId { get; private set; }
        public int StepOrder { get; private set; }
        public ApprovalDecision Decision { get; private set; }
        public string? Comment { get; private set; }
        public DateTime? DecidedAtUtc { get; private set; }
        public static ApprovalStep Create(Guid workflowId, Guid approverUserId, int stepOrder, DateTime nowUtc) => new() { Id = Guid.NewGuid(), WorkflowId = workflowId, ApproverUserId = approverUserId, StepOrder = stepOrder, Decision = ApprovalDecision.Pending };
        public void Record(ApprovalDecision decision, string? comment, DateTime nowUtc) { Decision = decision; Comment = comment?.Trim(); DecidedAtUtc = nowUtc; }
        public void Skip(DateTime nowUtc) 
        { 
            if (Decision == ApprovalDecision.Pending) 
            { 
                Decision = ApprovalDecision.Skipped; DecidedAtUtc = nowUtc; 
            } 
        }
    }
}
