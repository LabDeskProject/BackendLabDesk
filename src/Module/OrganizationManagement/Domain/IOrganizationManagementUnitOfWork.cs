namespace OrganizationManagement.Domain;

public interface IOrganizationManagementUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
