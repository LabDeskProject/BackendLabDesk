using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Domain.Interfaces
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
