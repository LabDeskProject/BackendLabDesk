using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Slices.ReassignRequest
{
    public sealed class ReassignRequestValidator : AbstractValidator<ReassignRequestCommand>
    {
        public ReassignRequestValidator() 
        { 
            RuleFor(x => x.AssignmentId).NotEmpty(); 
            RuleFor(x => x.AssigneeUserId).NotEmpty(); 
            RuleFor(x => x.TeamId).Must(x => x is null || x != Guid.Empty); 
            RuleFor(x => x.Note).MaximumLength(2000); 
        }
    }
}
