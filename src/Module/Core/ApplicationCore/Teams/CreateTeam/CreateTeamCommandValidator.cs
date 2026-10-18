using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Teams.CreateTeam
{
    public sealed class CreateTeamCommandValidator
    : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotEmpty()
                .WithMessage("OrganizationId is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Team name is required.")
                .MaximumLength(150)
                .WithMessage("Team name cannot exceed 150 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
