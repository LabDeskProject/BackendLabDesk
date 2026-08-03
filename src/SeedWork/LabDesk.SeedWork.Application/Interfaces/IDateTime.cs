using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Application.Interfaces
{
    public interface IDateTime
    {
        DateTime UtcNow {  get; }
    }
}
