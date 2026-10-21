using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowApproval.Application.Slices.StartApprovalWorkflow
{
    public sealed class StartApprovalWorkflowValidator : AbstractValidator<StartApprovalWorkflowCommand>
    {
        public StartApprovalWorkflowValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty(); 
            RuleFor(x => x.RequestId).NotEmpty();
            RuleFor(x => x.Mode).IsInEnum(); 
            RuleFor(x => x.ApproverSteps).NotEmpty();
            RuleForEach(x => x.ApproverSteps).Must(step => step.Count > 0 && step.All(id => id != Guid.Empty)).WithMessage("Every approval step must include valid approver IDs.");
            RuleFor(x => x.ApproverSteps).Must(steps => steps.SelectMany(x => x).Distinct().Count() == steps.Sum(x => x.Count)).WithMessage("An approver may appear only once in a workflow.");
        }
    }
}
