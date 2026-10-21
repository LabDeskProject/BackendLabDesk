using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.StartSlaClock
{
    public sealed class StartSlaClockValidator : AbstractValidator<StartSlaClockCommand>
    {
        public StartSlaClockValidator() 
        { 
            RuleFor(x => x.OrganizationId).NotEmpty(); 
            RuleFor(x => x.RequestId).NotEmpty(); 
            RuleFor(x => x.ServiceId).NotEmpty(); 
        }
    }
}
