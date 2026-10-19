using OrganizationManagement.Domain;
using DomainCore.Interfaces;

namespace OrganizationManagement.Infrastructure;

public sealed class OrganizationManagementUnitOfWork(OrganizationManagementDbContext dbContext) : IOrganizationManagementUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => _ = await dbContext.SaveChangesAsync(cancellationToken);
}
