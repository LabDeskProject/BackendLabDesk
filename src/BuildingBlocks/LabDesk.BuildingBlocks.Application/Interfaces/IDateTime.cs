using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Application.Interfaces
{
    public interface IDateTime
    {
        DateTime UtcNow {  get; }
    }
}
