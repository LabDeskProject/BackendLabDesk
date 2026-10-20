using BuildingBlocks.Application;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Services.ListOrganizationServices
{
    public sealed class ListOrganizationServicesHandler(IServiceQueries queries, IServiceRepository services, IUserContext user) : IRequestHandler<ListOrganizationServicesQuery, IReadOnlyList<ServiceListItem>>
    {
        public async Task<IReadOnlyList<ServiceListItem>> Handle(ListOrganizationServicesQuery request, CancellationToken ct)
        {
            if (!await services.IsOrganizationMemberAsync(request.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("User does not belong to this organization.");
            return await queries.ListByOrganizationAsync(request.OrganizationId, ct);
        }
    }

}
