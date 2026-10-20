using MediatR;

namespace ServiceCatalog.Application.Slices.GetCatalogService
{
    public sealed class GetCatalogServiceHandler(IServiceCatalogQueries queries) : IRequestHandler<GetCatalogServiceQuery, ServiceCatalogItem?>
    {
        public Task<ServiceCatalogItem?> Handle(GetCatalogServiceQuery request, CancellationToken ct) => queries.GetAsync(request.ServiceId, ct);
    }
}
