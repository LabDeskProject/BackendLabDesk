using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Teams.CreateTeam
{
    public sealed record CreateTeamResult(Guid Id, Guid OrganizationId, string Name);
}
