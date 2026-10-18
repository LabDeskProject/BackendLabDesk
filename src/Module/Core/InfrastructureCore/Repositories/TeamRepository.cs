using ApplicationCore.Interfaces;
using BuildingBlocks.Application;
using DomainCore.Teams;
using InfrastructureCore.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureCore.Repositories
{
    public sealed class TeamRepository
    : ITeamRepository, IUnitOfWorkIdentityAccess
    {
        private readonly CoreDbContext _dbContext;

        public TeamRepository(CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsByNameAsync(
            Guid organizationId,
            string name,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Teams
                .AnyAsync(
                    x =>
                        x.OrganizationId == organizationId
                        && x.Name == name,
                    cancellationToken);
        }

        public async Task AddAsync(
            Team team,
            CancellationToken cancellationToken)
        {
            await _dbContext.Teams.AddAsync(
                team,
                cancellationToken);

            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(
                cancellationToken);
        }
    }
}
