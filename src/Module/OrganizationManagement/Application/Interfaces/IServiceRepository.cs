using OrganizationManagement.Domain;

namespace OrganizationManagement.Application.Interfaces;

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
