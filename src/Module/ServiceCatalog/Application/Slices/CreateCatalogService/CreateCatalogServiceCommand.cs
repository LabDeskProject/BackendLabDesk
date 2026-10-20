using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCatalog.Application.Slices.CreateCatalogService
{
    public sealed record CreateCatalogServiceCommand(Guid OrganizationId, string Name, string? Description) : IRequest<Guid>;

}
