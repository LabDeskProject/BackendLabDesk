using RequestManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Interfaces
{
    public interface IRequestRepository
    {
        Task<bool> ServiceIsActiveInOrganizationAsync(Guid serviceId, Guid organizationId, CancellationToken ct);
        Task<bool> IsOrganizationMemberAsync(Guid organizationId, Guid userId, CancellationToken ct);
        Task<bool> IsOrganizationAdminAsync(Guid organizationId, Guid userId, CancellationToken ct);
        Task AddAsync(ServiceRequest request, CancellationToken ct);
        Task<ServiceRequest?> GetForUpdateAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(ServiceRequest request, CancellationToken ct);
    }
}
