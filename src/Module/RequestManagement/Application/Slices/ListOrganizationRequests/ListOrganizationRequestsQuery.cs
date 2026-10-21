using MediatR;
using RequestManagement.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Slices.ListOrganizationRequests
{
    public sealed record ListOrganizationRequestsQuery(Guid OrganizationId, int Page = 1, int PageSize = 50) : IRequest<IReadOnlyList<RequestListItem>>;
}
