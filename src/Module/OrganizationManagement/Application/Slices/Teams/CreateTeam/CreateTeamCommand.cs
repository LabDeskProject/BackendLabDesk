using BuildingBlocks.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Teams.CreateTeam
{
    public sealed record CreateTeamCommand(Guid OrganizationId, string Name, string? Description) : ICommand<CreateTeamResult>;
}
