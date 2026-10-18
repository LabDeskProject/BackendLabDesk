using ApplicationCore.Interfaces;
using BuildingBlocks.Application;
using DomainCore.Organizations;
using InfrastructureCore.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DomainCore.Organizations;

namespace InfrastructureCore.Repositories
{
    public sealed class OrganizationRepository
    : IOrganizationRepository, IUnitOfWorkIdentityAccess
    {
        private readonly CoreDbContext _dbContext;

        public OrganizationRepository(
            CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsBySlugAsync(
            string slug,
            CancellationToken cancellationToken)
        {
            return _dbContext.Organizations
                .AnyAsync(
                    x => x.Slug == slug,
                    cancellationToken);
        }

        public async Task AddAsync(
            Organization organization,
            CancellationToken cancellationToken)
        {
            await _dbContext.Organizations.AddAsync(
                organization,
                cancellationToken);
        }

        public async Task AddMemberAsync(
            OrganizationUser organizationUser,
            CancellationToken cancellationToken)
        {
            await _dbContext.OrganizationUsers.AddAsync(
                organizationUser,
                cancellationToken);
        }

        public Task SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            return _dbContext.SaveChangesAsync(
                cancellationToken);
        }

        public Task<bool> ExistsByIdAsync(
            Guid organizationId,
            CancellationToken cancellationToken)
        {
            return _dbContext.Organizations
                .AnyAsync(
                    organization => organization.Id == organizationId,
                    cancellationToken);
        }

    }
}
