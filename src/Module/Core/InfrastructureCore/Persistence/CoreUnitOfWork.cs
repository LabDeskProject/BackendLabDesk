using BuildingBlocks.Application;
using DomainCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureCore.Persistence
{
    public sealed class CoreUnitOfWork : IUnitOfWorkCore
    {
        private readonly CoreDbContext _dbContext;
        public CoreUnitOfWork(CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
