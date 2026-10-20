using ServiceCatalog.Domain;

namespace ServiceCatalog.Application;

public sealed record ServiceCatalogItem(Guid Id, Guid OrganizationId, string Name, string? Description, bool IsArchived);

public interface IServiceCatalogRepository
{
    Task<bool> OrganizationExistsAsync(Guid organizationId, CancellationToken ct);
    Task<bool> NameExistsAsync(Guid organizationId, string name, Guid? excludeId, CancellationToken ct);
    Task AddAsync(ServiceDefinition service, CancellationToken ct);
    Task<ServiceDefinition?> GetByIdAsync(Guid serviceId, CancellationToken ct);
    Task UpdateAsync(ServiceDefinition service, CancellationToken ct);
}

public interface IServiceCatalogQueries
{
    Task<ServiceCatalogItem?> GetAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<ServiceCatalogItem>> ListAsync(Guid organizationId, CancellationToken ct);
}

public interface IServiceCatalogUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct);
}
