using BuildingBlocks.Application;
using MediatR;
using SLA.Application.Contracts;
using SLA.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.ListSlaPolicies
{
    public sealed class ListSlaPoliciesHandler(ISlaRepository repo, ISlaAccess access, IUserContext user, ISlaUserContext global) : IRequestHandler<ListSlaPoliciesQuery, IReadOnlyList<SlaPolicyItem>>
    {
        public async Task<IReadOnlyList<SlaPolicyItem>> Handle(ListSlaPoliciesQuery q, CancellationToken ct) 
        {
            if (!global.IsAdmin && !await access.IsMemberAsync(q.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization membership is required."); 
            return await repo.ListPoliciesAsync(q.OrganizationId, ct); 
        }
    }
}
