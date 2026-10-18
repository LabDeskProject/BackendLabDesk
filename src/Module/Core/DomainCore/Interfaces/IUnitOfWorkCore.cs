using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Interfaces
{
    public interface IUnitOfWorkCore
    {
        Task SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
