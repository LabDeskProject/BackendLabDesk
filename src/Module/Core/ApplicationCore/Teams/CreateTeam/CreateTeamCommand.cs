using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Teams.CreateTeam
{
    public sealed record CreateTeamCommand(
    Guid OrganizationId,
    string Name,
    string? Description
) : IRequest<CreateTeamResult>;
}
