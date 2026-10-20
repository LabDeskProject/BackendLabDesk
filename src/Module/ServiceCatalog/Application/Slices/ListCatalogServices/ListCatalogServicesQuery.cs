using MediatR;
using ServiceCatalog.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Application.Slices.ListCatalogServices
{
    public sealed record ListCatalogServicesQuery(Guid OrganizationId) : IRequest<IReadOnlyList<ServiceCatalogItem>>;
}
