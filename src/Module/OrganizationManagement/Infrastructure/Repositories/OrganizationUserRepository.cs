using OrganizationManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.Domain;

namespace OrganizationManagement.Infrastructure.Repositories
{
    public sealed class OrganizationUserRepository(OrganizationManagementDbContext db) : IOrganizationUserRepository
    {
        public Task<bool> ExistsAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.AnyAsync(x => x.OrganizationId == organizationId && x.UserId == userId && x.RemovedAtUtc == null, ct);

        public Task<bool> IsAdminAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.AnyAsync(x => x.OrganizationId == organizationId && x.UserId == userId && x.RemovedAtUtc == null && x.Role == OrganizationRole.OrganizationAdmin, ct);

    }
}
