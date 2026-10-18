using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Application
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
