using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.ListSlaBreaches
{
    public sealed class ListSlaBreachesValidator : AbstractValidator<ListSlaBreachesQuery> 
    {
      public ListSlaBreachesValidator() 
      { 
            RuleFor(x => x.OrganizationId).NotEmpty(); 
            RuleFor(x => x.Page).GreaterThan(0); 
            RuleFor(x => x.PageSize).InclusiveBetween(1, 200); 
      } 
    }
}
