using LabDesk.Modules.Identity.Application.DTOs;
using LabDesk.Modules.Identity.Application.Interfaces;
using LabDesk.Modules.Identity.Application.Queries.GetUserById;
using LabDesk.Modules.Identity.Domain.IRepository;
using LabDesk.SeedWork.Application.CQRS;
using LabDesk.SeedWork.Application.Results;
using LabDesk.SeedWork.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Queries.GetTeamsByOrganization
{
    public class GetTeamsByOrganizationQueryHandler : IQueryHandler<GetTeamsByOrganizationQuery, IReadOnlyList<TeamDto>>
    {
        private readonly IIdentityDbContext _identityDbContext;
        public GetTeamsByOrganizationQueryHandler(IIdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public async Task<Result<IReadOnlyList<TeamDto>>> Handle(GetTeamsByOrganizationQuery request, CancellationToken cancellationToken)
        {
            var teams = await _identityDbContext.Teams
            .AsNoTracking()
            .Where(x => x.OrganizationId == request.OrganizationId)
            .Select(x => new TeamDto(
                x.Id,
                x.OrganizationId,
                x.Name,
                x.Description,
                x.TeamLeadId))
            .ToListAsync(cancellationToken);

            return Result<IReadOnlyList<TeamDto>>.Success(teams);
        }
    }
}
