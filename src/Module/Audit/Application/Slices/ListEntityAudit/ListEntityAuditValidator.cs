using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Slices.ListEntityAudit
{
    public sealed class ListEntityAuditValidator : AbstractValidator<ListEntityAuditQuery> 
    { 
        public ListEntityAuditValidator() 
        { 
            RuleFor(x => x.OrganizationId).NotEmpty(); 
            RuleFor(x => x.EntityType).NotEmpty().MaximumLength(150); 
            RuleFor(x => x.EntityId).NotEmpty().MaximumLength(100); 
            RuleFor(x => x.Page).GreaterThan(0); RuleFor(x => x.PageSize).InclusiveBetween(1, 200); 
        } 
    }

}
