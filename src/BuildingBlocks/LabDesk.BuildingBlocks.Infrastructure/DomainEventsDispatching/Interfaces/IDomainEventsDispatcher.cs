using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Infrastructure.DomainEventsDispatching.Interfaces
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}
