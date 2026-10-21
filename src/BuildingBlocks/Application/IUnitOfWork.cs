namespace BuildingBlocks.Application;

/// <summary>Unit of work abstraction for IdentityAccess persistence only.</summary>
public interface IUnitOfWorkIdentityAccess
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
