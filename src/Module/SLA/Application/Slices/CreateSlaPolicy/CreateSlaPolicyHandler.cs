using BuildingBlocks.Application;
using MediatR;
using SLA.Application.Interfaces;
using SLA.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.CreateSlaPolicy
{
    public sealed class CreateSlaPolicyHandler(ISlaRepository repo, ISlaUnitOfWork unit, ISlaAccess access, IUserContext user, ISlaUserContext global) : IRequestHandler<CreateSlaPolicyCommand, Guid>
    {
        public async Task<Guid> Handle(CreateSlaPolicyCommand c, CancellationToken ct)
        {
            if (!global.IsAdmin && !await access.IsAdminAsync(c.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required to manage SLA policies.");
            if (!await repo.ServiceIsActiveAsync(c.OrganizationId, c.ServiceId, ct)) 
                throw new KeyNotFoundException("Active service not found in organization.");
            if (await repo.GetPolicyAsync(c.OrganizationId, c.ServiceId, ct) is not null) 
                throw new InvalidOperationException("An active SLA policy already exists for this service.");
            var policy = SlaPolicy.Create(c.OrganizationId, c.ServiceId, c.ResponseMinutes, c.ResolutionMinutes, DateTime.UtcNow);
            await repo.AddPolicyAsync(policy, ct); 
            await unit.SaveChangesAsync(ct); 
            return policy.Id;
        }
    }
}
