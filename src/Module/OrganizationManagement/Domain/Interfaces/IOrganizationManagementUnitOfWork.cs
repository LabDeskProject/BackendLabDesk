namespace OrganizationManagement.Domain.Interfaces;

public interface IOrganizationManagementUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
