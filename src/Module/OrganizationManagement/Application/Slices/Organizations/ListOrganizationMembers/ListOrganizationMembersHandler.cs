using BuildingBlocks.Application;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Organizations.ListOrganizationMembers
{
    public sealed class ListOrganizationMembersHandler(IOrganizationRepository organizations, IUserContext user) : IRequestHandler<ListOrganizationMembersQuery, IReadOnlyList<OrganizationMemberItem>>
    {
        public async Task<IReadOnlyList<OrganizationMemberItem>> Handle(ListOrganizationMembersQuery query, CancellationToken ct)
        {
            if (!await organizations.ExistsByIdAsync(query.OrganizationId, ct)) 
                throw new KeyNotFoundException("Organization not found.");
            if (!await organizations.IsAdminAsync(query.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required to list members.");
            return await organizations.ListMembersAsync(query.OrganizationId, ct);
        }
    }
}
