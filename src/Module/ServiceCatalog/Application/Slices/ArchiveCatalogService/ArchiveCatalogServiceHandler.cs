using MediatR;

namespace ServiceCatalog.Application.Slices.ArchiveCatalogService
{
    public sealed class ArchiveCatalogServiceHandler(IServiceCatalogRepository repo, IServiceCatalogUnitOfWork unit) : IRequestHandler<ArchiveCatalogServiceCommand>
    {
        public async Task Handle(ArchiveCatalogServiceCommand request, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(request.ServiceId, ct) ?? 
                throw new KeyNotFoundException("Service not found.");
            entity.Archive(DateTime.UtcNow);
            await repo.UpdateAsync(entity, ct);
            await unit.SaveChangesAsync(ct);
        }
    }
}
