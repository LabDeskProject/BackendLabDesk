using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Organizations.CreateOrganization
{
    public sealed class CreateOrganizationCommandValidator
    : AbstractValidator<CreateOrganizationCommand>
    {
        public CreateOrganizationCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Organization name is required.")
                .MaximumLength(200);

            RuleFor(x => x.Slug)
                .NotEmpty()
                .WithMessage("Organization slug is required.")
                .MaximumLength(100)
                .Matches("^[a-z0-9-]+$")
                .WithMessage(
                    "Slug can only contain lowercase letters, numbers, and hyphens.");

            RuleFor(x => x.Description)
                .MaximumLength(1000);
        }
    }
}
