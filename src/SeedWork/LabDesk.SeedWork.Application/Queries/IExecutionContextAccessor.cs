using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Application.Queries
{
    public interface IExecutionContextAccessor
    {
        Guid UserId { get; }
        Guid CorrectionId { get; }
        bool IsAvailable { get; }
    }
}
