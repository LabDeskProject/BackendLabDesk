using BuildingBlocks.Application;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Organizations.ListMyOrganizations
{
    public sealed class ListMyOrganizationsHandler(IOrganizationRepository organizations, IUserContext user) : IRequestHandler<ListMyOrganizationsQuery, IReadOnlyList<OrganizationListItem>>
    {
        public Task<IReadOnlyList<OrganizationListItem>> Handle(ListMyOrganizationsQuery query, CancellationToken ct) => organizations.ListForUserAsync(user.UserId, ct);
    }

}
