using LabDesk.Modules.Identity.Domain.Aggregates;
using LabDesk.Modules.Identity.Domain.IRepository;
using LabDesk.SeedWork.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Infrastructure.Persistence.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IdentityDbContext _identityDbContext;
        public OrganizationRepository(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }
        public IUnitOfWork UnitOfWork => _identityDbContext;

        public void Add(Organization organization)
        {
            _identityDbContext.Organizations.Add(organization);
        }

        public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _identityDbContext.Organizations.AnyAsync(x => x.Slug == slug.ToLowerInvariant().Trim(), cancellationToken);
        }

        public async Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _identityDbContext.Organizations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Organization?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _identityDbContext.Organizations.FirstOrDefaultAsync(x => x.Slug == slug);
        }

        public void Update(Organization organization)
        {
            _identityDbContext.Organizations.Update(organization);
        }
    }
}
