using BuildingBlocks.Application.CQRS;
using OrganizationManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Services.ListOrganizationServices
{
    public sealed record ListOrganizationServicesQuery(Guid OrganizationId) : IQuery<IReadOnlyList<ServiceListItem>>;
}
