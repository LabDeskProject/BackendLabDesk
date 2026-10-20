using MediatR;
using ServiceCatalog.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Application.Slices.RenameCatalogService
{
    public sealed class RenameCatalogServiceHandler(IServiceCatalogRepository repo, IServiceCatalogUnitOfWork unit) : IRequestHandler<RenameCatalogServiceCommand>
    {
        public async Task Handle(RenameCatalogServiceCommand request, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(request.ServiceId, ct) ?? 
                throw new KeyNotFoundException("Service not found.");
            if (await repo.NameExistsAsync(entity.OrganizationId, request.Name.Trim(), entity.Id, ct)) 
                throw new InvalidOperationException("Service name already exists in the organization.");
            entity.Rename(request.Name, request.Description, DateTime.UtcNow);
            await repo.UpdateAsync(entity, ct);
            await unit.SaveChangesAsync(ct);
        }
    }
}
