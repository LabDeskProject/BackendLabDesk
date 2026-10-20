using Dapper;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace OrganizationManagement.Infrastructure.Repositories
{
    public sealed class OrganizationRepository(OrganizationManagementDbContext db) : IOrganizationRepository
    {
        public Task AddAsync(Organization organization, CancellationToken ct) => db.Organizations.AddAsync(organization, ct).AsTask();

        public Task AddMemberAsync(OrganizationUser member, CancellationToken ct) => db.OrganizationUsers.AddAsync(member, ct).AsTask();

        public Task<bool> ExistsByIdAsync(Guid organizationId, CancellationToken ct) => db.Organizations.AnyAsync(x => x.Id == organizationId && !x.IsArchived, ct);

        public Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct) => db.Organizations.AnyAsync(x => x.Slug == slug.Trim().ToLowerInvariant() && !x.IsArchived, ct);

        public Task<OrganizationUser?> GetAnyMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.SingleOrDefaultAsync(x => x.OrganizationId == organizationId && x.UserId == userId, ct);

        public Task<OrganizationUser?> GetMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.SingleOrDefaultAsync(x => x.OrganizationId == organizationId && x.UserId == userId && x.RemovedAtUtc == null, ct);

        public Task<bool> IsAdminAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.AnyAsync(x => x.OrganizationId == organizationId && x.UserId == userId && x.RemovedAtUtc == null && x.Role == OrganizationRole.OrganizationAdmin, ct);

        public Task<bool> IsMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.AnyAsync(x => x.OrganizationId == organizationId && x.UserId == userId && x.RemovedAtUtc == null, ct);

        public async Task<IReadOnlyList<OrganizationListItem>> ListForUserAsync(Guid userId, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            try { return (await c.QueryAsync<OrganizationListItem>(new CommandDefinition("SELECT o.Id,o.Name,o.Slug,m.Role,m.JoinedAt FROM organization_management.Organizations o INNER JOIN organization_management.OrganizationUsers m ON m.OrganizationId=o.Id WHERE m.UserId=@userId AND m.RemovedAtUtc IS NULL AND o.IsArchived=0 ORDER BY o.Name,o.Id", new { userId }, cancellationToken: ct))).AsList(); }
            finally { if (opened) await db.Database.CloseConnectionAsync(); }
        }

        public async Task<IReadOnlyList<OrganizationUser>> ListMemberEntitiesAsync(Guid organizationId, CancellationToken ct) => await db.OrganizationUsers.Where(x => x.OrganizationId == organizationId && x.RemovedAtUtc == null).ToListAsync(ct);


        public async Task<IReadOnlyList<OrganizationMemberItem>> ListMembersAsync(Guid organizationId, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            try { return (await c.QueryAsync<OrganizationMemberItem>(new CommandDefinition("SELECT OrganizationId,UserId,Role,JoinedAt FROM organization_management.OrganizationUsers WHERE OrganizationId=@organizationId AND RemovedAtUtc IS NULL ORDER BY JoinedAt,UserId", new { organizationId }, cancellationToken: ct))).AsList(); }
            finally { if (opened) await db.Database.CloseConnectionAsync(); }
        }
    }
}
