using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Memberships.ChangeOrganizationMemberRole
{
    public sealed class ChangeOrganizationMemberRoleValidator : AbstractValidator<ChangeOrganizationMemberRoleCommand>
    {
        public ChangeOrganizationMemberRoleValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Role).IsInEnum();
        }
    }
}
