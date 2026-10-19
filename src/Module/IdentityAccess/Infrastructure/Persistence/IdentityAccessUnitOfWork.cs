using BuildingBlocks.Application;
using BuildingBlocks.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence
{
    public sealed class IdentityAccessUnitOfWork: IUnitOfWorkIdentityAccess
    {
        private readonly IdentityAccessDbContext _dbContext;

        public IdentityAccessUnitOfWork(
            IdentityAccessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync(
            CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
