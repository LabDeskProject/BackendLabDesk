using MediatR;
using ServiceCatalog.Application;
using ServiceCatalog.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Application.Slices.CreateCatalogService
{
    public sealed class CreateCatalogServiceHandler(IServiceCatalogRepository repo, IServiceCatalogUnitOfWork unit) : IRequestHandler<CreateCatalogServiceCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCatalogServiceCommand request, CancellationToken ct)
        {
            if (!await repo.OrganizationExistsAsync(request.OrganizationId, ct)) 
                throw new KeyNotFoundException("Organization not found or archived.");
            if (await repo.NameExistsAsync(request.OrganizationId, request.Name.Trim(), null, ct)) 
                throw new InvalidOperationException("Service name already exists in the organization.");
            var entity = ServiceDefinition.Create(request.OrganizationId, request.Name, request.Description, DateTime.UtcNow);
            await repo.AddAsync(entity, ct);
            await unit.SaveChangesAsync(ct);
            return entity.Id;
        }
    }
}
