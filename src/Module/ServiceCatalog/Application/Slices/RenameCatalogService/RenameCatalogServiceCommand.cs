using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Application.Slices.RenameCatalogService
{
    public sealed record RenameCatalogServiceCommand(Guid ServiceId, string Name, string? Description) : IRequest;
}
