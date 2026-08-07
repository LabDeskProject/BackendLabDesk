using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Domain.Interfaces
{
    public interface IBusinessRule
    {
        bool IsBroken();

        string Message { get; }
    }
}
