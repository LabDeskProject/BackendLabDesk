using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Application.Slices.ArchiveCatalogService
{
    public sealed record ArchiveCatalogServiceCommand(Guid ServiceId) : IRequest;
}
