using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Application
{
    public interface IUnitOfWorkIdentityAccess
    {
        Task SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
