using LabDesk.Modules.Identity.Application.DTOs;
using LabDesk.SeedWork.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Queries.GetTeamsByOrganization
{
    public record GetTeamsByOrganizationQuery(Guid OrganizationId) : IQuery<IReadOnlyList<TeamDto>>;
}
