using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace OrganizationManagement.Infrastructure.Repositories
{
    public sealed class ServiceRepository(OrganizationManagementDbContext db) : IServiceRepository
    {
        public Task AddAsync(Service service, CancellationToken ct) => db.Services.AddAsync(service, ct).AsTask();

        Task<bool> IServiceRepository.ExistsByNameAsync(Guid organizationId, string name, CancellationToken ct) => db.Services.AnyAsync(x => x.OrganizationId == organizationId && x.Name == name.Trim() && x.IsActive, ct);

        public Task<Service?> GetByIdAsync(Guid serviceId, CancellationToken ct) => db.Services.SingleOrDefaultAsync(x => x.Id == serviceId, ct);

        public Task<bool> IsOrganizationAdminAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.AnyAsync(x => x.OrganizationId == organizationId && x.UserId == userId && x.RemovedAtUtc == null && x.Role == OrganizationRole.OrganizationAdmin, ct);

        public Task<bool> IsOrganizationMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.AnyAsync(x => x.OrganizationId == organizationId && x.UserId == userId && x.RemovedAtUtc == null, ct);

        public Task<bool> OrganizationExistsAsync(Guid organizationId, CancellationToken ct) => db.Organizations.AnyAsync(x => x.Id == organizationId && !x.IsArchived, ct);

        public Task<bool> TeamBelongsToOrganizationAsync(Guid teamId, Guid organizationId, CancellationToken ct) => db.Teams.AnyAsync(x => x.Id == teamId && x.OrganizationId == organizationId && !x.IsArchived, ct);
    }
}
