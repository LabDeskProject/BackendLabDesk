using MediatR;
using OrganizationManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Memberships.AddOrganizationMember
{
    public sealed record AddOrganizationMemberCommand(Guid OrganizationId, Guid UserId, OrganizationRole Role) : IRequest;
}
