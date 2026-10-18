using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Domain
{
    public interface IDomainEvent
    {
        DateTimeOffset OccurredAt { get; }
    }
}
