using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Memberships.RemoveOrganizationMember
{
    public sealed record RemoveOrganizationMemberCommand(Guid OrganizationId, Guid UserId) : IRequest;
}
