using BuildingBlocks.Application;
using MediatR;
using SLA.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.UpdateSlaPolicy
{
    public sealed class UpdateSlaPolicyHandler(ISlaRepository repo, ISlaUnitOfWork unit, ISlaAccess access, IUserContext user, ISlaUserContext global) : IRequestHandler<UpdateSlaPolicyCommand>
    {
        public async Task Handle(UpdateSlaPolicyCommand c, CancellationToken ct)
        {
            var p = await repo.GetPolicyByIdAsync(c.PolicyId, ct) ?? 
                throw new KeyNotFoundException("SLA policy not found.");
            if (!global.IsAdmin && !await access.IsAdminAsync(p.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required to manage SLA policies.");
            p.Update(c.ResponseMinutes, c.ResolutionMinutes, DateTime.UtcNow); 
            await unit.SaveChangesAsync(ct);
        }
    }
}
