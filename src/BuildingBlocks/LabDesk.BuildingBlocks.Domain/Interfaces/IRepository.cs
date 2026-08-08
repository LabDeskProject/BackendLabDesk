using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Domain.Interfaces
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
