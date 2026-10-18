using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IUnitOfWorkIdentityAccess
    {
        Task SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
