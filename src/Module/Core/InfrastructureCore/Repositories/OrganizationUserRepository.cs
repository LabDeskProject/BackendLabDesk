using ApplicationCore.Interfaces;
using BuildingBlocks.Application;
using InfrastructureCore.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureCore.Repositories
{
    public sealed class OrganizationUserRepository
    : IOrganizationUserRepository, IUnitOfWorkIdentityAccess
    {
        private readonly CoreDbContext _dbContext;

        public OrganizationUserRepository(
            CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(
            Guid organizationId,
            Guid userId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.OrganizationUsers
                .AnyAsync(
                    x =>
                        x.OrganizationId == organizationId
                        && x.UserId == userId,
                    cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(
                cancellationToken);
        }
    }
}
