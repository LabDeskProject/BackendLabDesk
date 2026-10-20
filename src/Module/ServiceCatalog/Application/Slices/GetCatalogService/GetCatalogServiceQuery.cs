using MediatR;
using ServiceCatalog.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Application.Slices.GetCatalogService
{
    public sealed record GetCatalogServiceQuery(Guid ServiceId) : IRequest<ServiceCatalogItem?>;
}
