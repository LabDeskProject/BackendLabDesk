using LabDesk.SeedWork.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Commands.CreateTeam
{
    public record CreateTeamCommand(
     Guid OrganizationId,
     string Name,
     string? Description = null,
     Guid? TeamLeadId = null) : ICommand<Guid>;
}
