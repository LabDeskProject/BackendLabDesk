using MediatR;

namespace ServiceCatalog.Application.Slices.ListCatalogServices
{
    public sealed class ListCatalogServicesHandler(IServiceCatalogQueries queries) : IRequestHandler<ListCatalogServicesQuery, IReadOnlyList<ServiceCatalogItem>>
    {
        public Task<IReadOnlyList<ServiceCatalogItem>> Handle(ListCatalogServicesQuery request, CancellationToken ct) => queries.ListAsync(request.OrganizationId, ct);
    }
}
