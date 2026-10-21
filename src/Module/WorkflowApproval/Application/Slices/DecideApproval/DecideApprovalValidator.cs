using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowApproval.Domain.Enum;

namespace WorkflowApproval.Application.Slices.DecideApproval
{
    public sealed class DecideApprovalValidator : AbstractValidator<DecideApprovalCommand>
    {
        public DecideApprovalValidator() 
        { 
            RuleFor(x => x.WorkflowId).NotEmpty(); 
            RuleFor(x => x.Decision).Must(x => x is ApprovalDecision.Approved or ApprovalDecision.Rejected); 
            RuleFor(x => x.Comment).MaximumLength(2000); }
    }
}
