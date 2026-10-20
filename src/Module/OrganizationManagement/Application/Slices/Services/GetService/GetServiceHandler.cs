using BuildingBlocks.Application;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Services.GetService
{
    public sealed class GetServiceHandler(IServiceQueries queries, IServiceRepository services, IUserContext user) : IRequestHandler<GetServiceQuery, ServiceListItem?>
    {
        public async Task<ServiceListItem?> Handle(GetServiceQuery request, CancellationToken ct)
        {
            var result = await queries.GetDetailsAsync(request.ServiceId, ct);
            if (result is null) return null;
            if (!await services.IsOrganizationMemberAsync(result.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("User does not belong to this organization.");
            return result;
        }
    }
}
