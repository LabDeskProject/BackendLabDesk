using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Memberships.AddOrganizationMember
{
    public sealed class AddOrganizationMemberValidator : AbstractValidator<AddOrganizationMemberCommand>
    {
        public AddOrganizationMemberValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Role).IsInEnum();
        }
    }
}
