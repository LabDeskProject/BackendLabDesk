using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Slices.ListOrganizationAudit
{
    public sealed class ListOrganizationAuditValidator : AbstractValidator<ListOrganizationAuditQuery> 
    { 
        public ListOrganizationAuditValidator() 
        {
            RuleFor(x => x.OrganizationId).NotEmpty();
            RuleFor(x => x.Page).GreaterThan(0); 
            RuleFor(x => x.PageSize).InclusiveBetween(1, 200); 
        } 
    }

}
