using SLA.Application.Contracts;
using SLA.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Interfaces
{
    public interface ISlaRepository
    {
        Task<bool> ServiceIsActiveAsync(Guid organizationId, Guid serviceId, CancellationToken ct);
        Task<bool> RequestUsesServiceAsync(Guid organizationId, Guid requestId, Guid serviceId, CancellationToken ct);
        Task<SlaPolicy?> GetPolicyAsync(Guid organizationId, Guid serviceId, CancellationToken ct);
        Task<SlaPolicy?> GetPolicyByIdAsync(Guid id, CancellationToken ct);
        Task<SlaClock?> GetClockForRequestAsync(Guid requestId, CancellationToken ct);
        Task<SlaClock?> GetClockByIdAsync(Guid id, CancellationToken ct);
        Task AddPolicyAsync(SlaPolicy policy, CancellationToken ct);
        Task AddClockAsync(SlaClock clock, CancellationToken ct);
        Task<IReadOnlyList<SlaPolicyItem>> ListPoliciesAsync(Guid organizationId, CancellationToken ct);
        Task<IReadOnlyList<SlaClockItem>> ListBreachesAsync(Guid organizationId, int skip, int take, CancellationToken ct);
    }
}
