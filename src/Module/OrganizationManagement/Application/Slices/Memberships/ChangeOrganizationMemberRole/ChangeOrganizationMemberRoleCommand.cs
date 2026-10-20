using MediatR;
using OrganizationManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Memberships.ChangeOrganizationMemberRole
{
    public sealed record ChangeOrganizationMemberRoleCommand(Guid OrganizationId, Guid UserId, OrganizationRole Role) : IRequest;

}
