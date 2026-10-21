using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Interfaces
{
    public interface ISlaAccess 
    { 
        Task<bool> IsMemberAsync(Guid organizationId, Guid userId, CancellationToken ct); 
        Task<bool> IsAdminAsync(Guid organizationId, Guid userId, CancellationToken ct); 
    }

}
