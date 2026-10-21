using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Interfaces
{
    public interface IAssignmentUnitOfWork 
    { 
        Task SaveChangesAsync(CancellationToken ct); 
    }
}
