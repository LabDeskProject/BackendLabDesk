using LabDesk.Modules.Identity.Application.DTOs;
using LabDesk.Modules.Identity.Application.Interfaces;
using LabDesk.SeedWork.Application.CQRS;
using LabDesk.SeedWork.Application.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Queries.GetOrganizationBySlug
{
    public class GetOrganizationBySlugQueryHandler : IQueryHandler<GetOrganizationBySlugQuery, OrganizationDto?>
    {
        private readonly IIdentityDbContext _identityDbContext;
        public GetOrganizationBySlugQueryHandler(IIdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public async Task<Result<OrganizationDto?>> Handle(GetOrganizationBySlugQuery request, CancellationToken cancellationToken)
        {
            var slugNormalized = request.Slug.ToLowerInvariant().Trim();

            var organization = await _identityDbContext.Organizations
                .AsNoTracking()
                .Where(x => x.Slug == slugNormalized)
                .Select(x => new OrganizationDto(
                    x.Id,
                    x.Name,
                    x.Slug,
                    x.Description,
                    x.CreatedAt))
                .FirstOrDefaultAsync(cancellationToken);

            return Result<OrganizationDto?>.Success(organization);
        }
    }
}
