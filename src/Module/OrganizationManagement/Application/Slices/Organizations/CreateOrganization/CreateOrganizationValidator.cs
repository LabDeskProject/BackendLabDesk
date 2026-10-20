using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Organizations.CreateOrganization
{
    public sealed class CreateOrganizationValidator : AbstractValidator<CreateOrganizationCommand>
    {
        public CreateOrganizationValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}
