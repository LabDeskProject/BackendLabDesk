using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Interfaces
{
    public interface IAuditAccess 
    { 
        Task<bool> IsOrganizationMemberAsync(Guid organizationId, Guid userId, CancellationToken ct); 
        Task<bool> IsOrganizationAdminAsync(Guid organizationId, Guid userId, CancellationToken ct); 
    }

}
