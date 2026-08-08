using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Domain.Interfaces
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
