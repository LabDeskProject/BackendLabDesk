using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Interfaces
{
    public interface ISlaUnitOfWork 
    { 
        Task SaveChangesAsync(CancellationToken ct);
    }

}
