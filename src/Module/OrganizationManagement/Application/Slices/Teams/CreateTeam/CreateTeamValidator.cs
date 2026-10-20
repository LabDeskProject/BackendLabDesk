using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Teams.CreateTeam
{
    public sealed class CreateTeamValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }
}
