using BuildingBlocks.Application;
using MediatR;
using SLA.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.ArchiveSlaPolicy
{
    public sealed class ArchiveSlaPolicyHandler(ISlaRepository repo, ISlaUnitOfWork unit, ISlaAccess access, IUserContext user, ISlaUserContext global) : IRequestHandler<ArchiveSlaPolicyCommand>
    {
        public async Task Handle(ArchiveSlaPolicyCommand c, CancellationToken ct)
        {
            var p = await repo.GetPolicyByIdAsync(c.PolicyId, ct) ?? 
                throw new KeyNotFoundException("SLA policy not found."); 
            if (!global.IsAdmin && !await access.IsAdminAsync(p.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required to manage SLA policies."); 
            p.Archive(DateTime.UtcNow); 
            await unit.SaveChangesAsync(ct);
        }
    }
}
