using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Teams.CreateTeam
{
    public sealed record CreateTeamResult(
    Guid TeamId,
    Guid OrganizationId,
    string Name
);
}
