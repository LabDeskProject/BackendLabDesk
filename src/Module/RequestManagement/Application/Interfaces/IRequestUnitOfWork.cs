using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Interfaces
{
    public interface IRequestUnitOfWork 
    {
        Task SaveChangesAsync(CancellationToken ct); 
    }
}
