using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}
