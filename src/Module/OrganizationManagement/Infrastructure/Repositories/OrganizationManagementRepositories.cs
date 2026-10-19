using Dapper;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using System.Data;
using DomainCore.Organizations;
namespace OrganizationManagement.Infrastructure.Repositories;
public sealed class OrganizationManagementRepositories(OrganizationManagementDbContext db) : IOrganizationRepository, IOrganizationUserRepository, ITeamRepository, IServiceRepository, IServiceQueries
{
    public Task<bool> ExistsByIdAsync(Guid organizationId, CancellationToken ct) => db.Organizations.AnyAsync(x => x.Id == organizationId && !x.IsArchived, ct);
    public Task<bool> ExistsByNameAsync(Guid organizationId, string name, CancellationToken ct) => db.Services.AnyAsync(x => x.OrganizationId == organizationId && x.Name == name.Trim() && x.IsActive, ct);
    public Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct) => db.Organizations.AnyAsync(x => x.Slug == slug.Trim().ToLowerInvariant() && !x.IsArchived, ct);
    public Task AddAsync(Organization organization, CancellationToken ct) => db.Organizations.AddAsync(organization, ct).AsTask();
    public Task AddMemberAsync(OrganizationUser member, CancellationToken ct) => db.OrganizationUsers.AddAsync(member, ct).AsTask();
    public Task<bool> ExistsAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.AnyAsync(x => x.OrganizationId == organizationId && x.UserId == userId && x.RemovedAtUtc == null, ct);
    public Task<bool> IsAdminAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.AnyAsync(x => x.OrganizationId == organizationId && x.UserId == userId && x.RemovedAtUtc == null && x.Role == OrganizationRole.OrganizationAdmin, ct);
    public async Task<IReadOnlyList<OrganizationUser>> ListMemberEntitiesAsync(Guid organizationId, CancellationToken ct) => await db.OrganizationUsers.Where(x => x.OrganizationId == organizationId && x.RemovedAtUtc == null).ToListAsync(ct);
    public Task<OrganizationUser?> GetMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.SingleOrDefaultAsync(x => x.OrganizationId == organizationId && x.UserId == userId && x.RemovedAtUtc == null, ct);
    public Task<OrganizationUser?> GetAnyMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.OrganizationUsers.SingleOrDefaultAsync(x => x.OrganizationId == organizationId && x.UserId == userId, ct);
    public Task<bool> IsMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => ExistsAsync(organizationId, userId, ct);
    Task<bool> ITeamRepository.ExistsByNameAsync(Guid organizationId, string name, CancellationToken ct) => db.Teams.AnyAsync(x => x.OrganizationId == organizationId && x.Name == name.Trim() && !x.IsArchived, ct);
    public Task AddAsync(DomainCore.Teams.Team team, CancellationToken ct) => db.Teams.AddAsync(team, ct).AsTask();
    Task IServiceRepository.AddAsync(DomainCore.Service.Service service, CancellationToken ct) => db.Services.AddAsync(service, ct).AsTask();
    public Task<DomainCore.Service.Service?> GetByIdAsync(Guid serviceId, CancellationToken ct) => db.Services.SingleOrDefaultAsync(x => x.Id == serviceId, ct);
    Task<bool> IServiceRepository.ExistsByNameAsync(Guid organizationId, string name, CancellationToken ct) => db.Services.AnyAsync(x => x.OrganizationId == organizationId && x.Name == name.Trim() && x.IsActive, ct);
    public Task<bool> OrganizationExistsAsync(Guid organizationId, CancellationToken ct) => ExistsByIdAsync(organizationId, ct);
    public Task<bool> IsOrganizationMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => ExistsAsync(organizationId, userId, ct);
    public Task<bool> IsOrganizationAdminAsync(Guid organizationId, Guid userId, CancellationToken ct) => IsAdminAsync(organizationId, userId, ct);
    public Task<bool> TeamBelongsToOrganizationAsync(Guid teamId, Guid organizationId, CancellationToken ct) => db.Teams.AnyAsync(x => x.Id == teamId && x.OrganizationId == organizationId && !x.IsArchived, ct);
    public async Task<IReadOnlyList<OrganizationListItem>> ListForUserAsync(Guid userId, CancellationToken ct)
    {
        var c = db.Database.GetDbConnection(); var opened = c.State != ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
        try { return (await c.QueryAsync<OrganizationListItem>(new CommandDefinition("SELECT o.Id,o.Name,o.Slug,m.Role,m.JoinedAt FROM organization_management.Organizations o INNER JOIN organization_management.OrganizationUsers m ON m.OrganizationId=o.Id WHERE m.UserId=@userId AND m.RemovedAtUtc IS NULL AND o.IsArchived=0 ORDER BY o.Name,o.Id", new { userId }, cancellationToken: ct))).AsList(); }
        finally { if (opened) await db.Database.CloseConnectionAsync(); }
    }
    public async Task<IReadOnlyList<OrganizationMemberItem>> ListMembersAsync(Guid organizationId, CancellationToken ct)
    {
        var c = db.Database.GetDbConnection(); var opened = c.State != ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
        try { return (await c.QueryAsync<OrganizationMemberItem>(new CommandDefinition("SELECT OrganizationId,UserId,Role,JoinedAt FROM organization_management.OrganizationUsers WHERE OrganizationId=@organizationId AND RemovedAtUtc IS NULL ORDER BY JoinedAt,UserId", new { organizationId }, cancellationToken: ct))).AsList(); }
        finally { if (opened) await db.Database.CloseConnectionAsync(); }
    }
    public async Task<IReadOnlyList<ServiceListItem>> ListByOrganizationAsync(Guid organizationId, CancellationToken ct)
    {
        var c = db.Database.GetDbConnection(); var opened = c.State != ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
        try { return (await c.QueryAsync<ServiceListItem>(new CommandDefinition("SELECT Id,OrganizationId,ResponsibleTeamId,Name,Description,IsActive FROM organization_management.Services WHERE OrganizationId=@organizationId AND IsActive=1 ORDER BY Name,Id", new { organizationId }, cancellationToken: ct))).AsList(); }
        finally { if (opened) await db.Database.CloseConnectionAsync(); }
    }
    public async Task<ServiceListItem?> GetDetailsAsync(Guid serviceId, CancellationToken ct)
    {
        var c = db.Database.GetDbConnection(); var opened = c.State != ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
        try { return await c.QuerySingleOrDefaultAsync<ServiceListItem>(new CommandDefinition("SELECT Id,OrganizationId,ResponsibleTeamId,Name,Description,IsActive FROM organization_management.Services WHERE Id=@serviceId AND IsActive=1", new { serviceId }, cancellationToken: ct)); }
        finally { if (opened) await db.Database.CloseConnectionAsync(); }
    }
}
