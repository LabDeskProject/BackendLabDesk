using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.UpdateSlaPolicy
{
    public sealed class UpdateSlaPolicyValidator : AbstractValidator<UpdateSlaPolicyCommand>
    {
        public UpdateSlaPolicyValidator() 
        { 
            RuleFor(x => x.PolicyId).NotEmpty(); 
            RuleFor(x => x.ResponseMinutes).GreaterThan(0); 
            RuleFor(x => x.ResolutionMinutes).GreaterThanOrEqualTo(x => x.ResponseMinutes); 
        }
    }
}
