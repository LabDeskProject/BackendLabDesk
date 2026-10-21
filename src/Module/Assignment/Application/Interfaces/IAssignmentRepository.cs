using Assignment.Application.Contracts;
using Assignment.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<bool> RequestExistsAsync(Guid requestId, Guid organizationId, CancellationToken ct);
        Task<bool> AssigneeIsMemberAsync(Guid userId, Guid organizationId, CancellationToken ct);
        Task<bool> IsOrganizationMemberAsync(Guid userId, Guid organizationId, CancellationToken ct);
        Task<bool> IsOrganizationAdminAsync(Guid userId, Guid organizationId, CancellationToken ct);
        Task<bool> IsRequestInOrganizationAsync(Guid requestId, Guid organizationId, CancellationToken ct);
        Task<bool> IsTeamInOrganizationAsync(Guid teamId, Guid organizationId, CancellationToken ct);
        Task<Guid?> GetRequestOrganizationAsync(Guid requestId, CancellationToken ct);
        Task<AssignmentRecord?> GetActiveForRequestAsync(Guid requestId, CancellationToken ct);
        Task<AssignmentRecord?> GetActiveForRequestAsyncById(Guid assignmentId, CancellationToken ct);
        Task AddAsync(AssignmentRecord assignment, CancellationToken ct);
        Task UpdateAsync(AssignmentRecord assignment, CancellationToken ct);
        Task<IReadOnlyList<AssignmentListItem>> ListForRequestAsync(Guid requestId, CancellationToken ct);
        Task<IReadOnlyList<AssignmentListItem>> ListForUserAsync(Guid organizationId, Guid userId, int skip, int take, CancellationToken ct);
    }
}
