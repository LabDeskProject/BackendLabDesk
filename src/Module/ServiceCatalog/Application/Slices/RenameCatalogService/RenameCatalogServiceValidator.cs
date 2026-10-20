using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Application.Slices.RenameCatalogService
{
    public sealed class RenameCatalogServiceValidator : AbstractValidator<RenameCatalogServiceCommand>
    {
        public RenameCatalogServiceValidator() 
        { 
            RuleFor(x => x.ServiceId).NotEmpty(); 
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150); 
            RuleFor(x => x.Description).MaximumLength(1000); 
        }
    }
}
