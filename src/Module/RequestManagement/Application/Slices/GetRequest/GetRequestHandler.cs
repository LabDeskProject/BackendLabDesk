using MediatR;
using RequestManagement.Application.Contracts;
using RequestManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Slices.GetRequest
{
    public sealed class GetRequestHandler(IRequestQueries queries, IRequestRepository requests, IRequestUserContext user) : IRequestHandler<GetRequestQuery, RequestListItem?>
    {
        public async Task<RequestListItem?> Handle(GetRequestQuery request, CancellationToken ct)
        {
            var item = await queries.GetAsync(request.RequestId, ct);
            if (item is null) return null;
            if (!await requests.IsOrganizationMemberAsync(item.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("User does not belong to the request organization.");
            if (item.SubmittedByUserId != user.UserId && !await requests.IsOrganizationAdminAsync(item.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("Request access is restricted to its submitter or organization admins.");
            return item;
        }
    }
}
