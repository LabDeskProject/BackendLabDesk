using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.ListSlaPolicies
{
    public sealed class ListSlaPoliciesValidator : AbstractValidator<ListSlaPoliciesQuery> 
    { 
        public ListSlaPoliciesValidator() => RuleFor(x => x.OrganizationId).NotEmpty(); 
    }
}
