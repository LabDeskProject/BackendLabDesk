using LabDesk.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Infrastructure.DomainEventsDispatching.Interfaces
{
    public interface IDomainEventsAccessor
    {
        IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

        void ClearAllDomainEvents();
    }
}
