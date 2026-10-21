using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowApproval.Application.Interfaces
{
    public interface IApprovalUnitOfWork 
    { 
        Task SaveChangesAsync(CancellationToken ct); 
    }
}
