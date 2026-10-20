using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Memberships.RemoveOrganizationMember
{
    public sealed class RemoveOrganizationMemberValidator : AbstractValidator<RemoveOrganizationMemberCommand>
    {
        public RemoveOrganizationMemberValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
