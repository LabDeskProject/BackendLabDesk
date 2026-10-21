using MediatR;
using RequestManagement.Application.Contracts;
using RequestManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Slices.ListOrganizationRequests
{
    public sealed class ListOrganizationRequestsHandler(IRequestQueries queries, IRequestRepository requests, IRequestUserContext user) : IRequestHandler<ListOrganizationRequestsQuery, IReadOnlyList<RequestListItem>>
    {
        public async Task<IReadOnlyList<RequestListItem>> Handle(ListOrganizationRequestsQuery request, CancellationToken ct)
        {
            if (!await requests.IsOrganizationMemberAsync(request.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("User does not belong to this organization.");
            var admin = await requests.IsOrganizationAdminAsync(request.OrganizationId, user.UserId, ct);
            return await queries.ListAsync(request.OrganizationId, user.UserId, admin, (request.Page - 1) * request.PageSize, request.PageSize, ct);
        }
    }

}
