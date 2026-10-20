using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Application.Slices.CreateCatalogService
{
    public sealed class CreateCatalogServiceValidator : AbstractValidator<CreateCatalogServiceCommand>
    {
        public CreateCatalogServiceValidator() 
        { 
            RuleFor(x => x.OrganizationId).NotEmpty(); 
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150); 
            RuleFor(x => x.Description).MaximumLength(1000); 
        }
    }
}
