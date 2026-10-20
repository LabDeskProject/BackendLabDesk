using MediatR;
using OrganizationManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Organizations.ListMyOrganizations
{
    public sealed record ListMyOrganizationsQuery : IRequest<IReadOnlyList<OrganizationListItem>>;
}
