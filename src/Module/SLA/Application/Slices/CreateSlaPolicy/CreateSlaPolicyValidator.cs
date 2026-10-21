using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.CreateSlaPolicy
{
    public sealed class CreateSlaPolicyValidator : AbstractValidator<CreateSlaPolicyCommand>
    {
        public CreateSlaPolicyValidator() 
        { 
            RuleFor(x => x.OrganizationId).NotEmpty(); 
            RuleFor(x => x.ServiceId).NotEmpty(); 
            RuleFor(x => x.ResponseMinutes).GreaterThan(0); 
            RuleFor(x => x.ResolutionMinutes).GreaterThanOrEqualTo(x => x.ResponseMinutes); 
        }
    }
}
