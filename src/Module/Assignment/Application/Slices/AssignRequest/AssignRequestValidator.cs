using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Slices.AssignRequest
{
    public sealed class AssignRequestValidator : AbstractValidator<AssignRequestCommand>
    {
        public AssignRequestValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty(); 
            RuleFor(x => x.RequestId).NotEmpty(); 
            RuleFor(x => x.AssigneeUserId).NotEmpty(); 
            RuleFor(x => x.TeamId).Must(x => x is null || x != Guid.Empty); 
            RuleFor(x => x.Note).MaximumLength(2000);
        }
    }
}
