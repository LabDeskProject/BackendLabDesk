using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Slices.ListOrganizationRequests
{
    public sealed class ListOrganizationRequestsValidator : AbstractValidator<ListOrganizationRequestsQuery>
    {
        public ListOrganizationRequestsValidator() 
        { 
            RuleFor(x => x.OrganizationId).NotEmpty();
            RuleFor(x => x.Page).GreaterThan(0); 
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100); 
        }
    }
}
