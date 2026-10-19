using DomainCore.Organizations;
using DomainCore.Service;
using DomainCore.Teams;
namespace OrganizationManagement.Application.Interfaces;
public sealed record ServiceListItem(Guid Id, Guid OrganizationId, Guid ResponsibleTeamId, string Name, string? Description, bool IsActive);
public sealed record OrganizationMemberItem(Guid OrganizationId, Guid UserId, OrganizationRole Role, DateTime JoinedAt);
public sealed record OrganizationListItem(Guid Id, string Name, string Slug, OrganizationRole Role, DateTime JoinedAt);
public interface IOrganizationRepository
{
    Task<bool> ExistsByIdAsync(Guid organizationId, CancellationToken ct);
    Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct);
    Task AddAsync(Organization organization, CancellationToken ct);
    Task AddMemberAsync(OrganizationUser member, CancellationToken ct);
    Task<IReadOnlyList<OrganizationUser>> ListMemberEntitiesAsync(Guid organizationId, CancellationToken ct);
    Task<OrganizationUser?> GetAnyMemberAsync(Guid organizationId, Guid userId, CancellationToken ct);
    Task<bool> IsMemberAsync(Guid organizationId, Guid userId, CancellationToken ct);
    Task<OrganizationUser?> GetMemberAsync(Guid organizationId, Guid userId, CancellationToken ct);
    Task<IReadOnlyList<OrganizationMemberItem>> ListMembersAsync(Guid organizationId, CancellationToken ct);
    Task<bool> IsAdminAsync(Guid organizationId, Guid userId, CancellationToken ct);
    Task<IReadOnlyList<OrganizationListItem>> ListForUserAsync(Guid userId, CancellationToken ct);
}
public interface IOrganizationUserRepository { Task<bool> ExistsAsync(Guid organizationId, Guid userId, CancellationToken ct); Task<bool> IsAdminAsync(Guid organizationId, Guid userId, CancellationToken ct); }
public interface ITeamRepository { Task<bool> ExistsByNameAsync(Guid organizationId, string name, CancellationToken ct); Task AddAsync(Team team, CancellationToken ct); }
public interface IServiceRepository
{
    Task AddAsync(Service service, CancellationToken ct);
    Task<Service?> GetByIdAsync(Guid serviceId, CancellationToken ct);
    Task<bool> ExistsByNameAsync(Guid organizationId, string name, CancellationToken ct);
    Task<bool> OrganizationExistsAsync(Guid organizationId, CancellationToken ct);
    Task<bool> TeamBelongsToOrganizationAsync(Guid teamId, Guid organizationId, CancellationToken ct);
    Task<bool> IsOrganizationMemberAsync(Guid organizationId, Guid userId, CancellationToken ct);
    Task<bool> IsOrganizationAdminAsync(Guid organizationId, Guid userId, CancellationToken ct);
}
public interface IServiceQueries { Task<IReadOnlyList<ServiceListItem>> ListByOrganizationAsync(Guid organizationId, CancellationToken ct); Task<ServiceListItem?> GetDetailsAsync(Guid serviceId, CancellationToken ct); }
