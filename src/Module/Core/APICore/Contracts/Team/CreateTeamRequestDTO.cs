using System;
using System.Collections.Generic;
using System.Text;

namespace APICore.Contracts.Team
{
    public sealed record CreateTeamRequest(
    Guid OrganizationId,
    string Name,
    string? Description
);
}
