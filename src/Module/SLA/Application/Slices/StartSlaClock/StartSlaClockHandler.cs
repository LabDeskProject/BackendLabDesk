using BuildingBlocks.Application;
using MediatR;
using SLA.Application.Interfaces;
using SLA.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.StartSlaClock
{
    public sealed class StartSlaClockHandler(ISlaRepository repo, ISlaUnitOfWork unit, ISlaAccess access, IUserContext user, ISlaUserContext global) : IRequestHandler<StartSlaClockCommand, Guid>
    {
        public async Task<Guid> Handle(StartSlaClockCommand c, CancellationToken ct)
        {
            if (!global.IsAdmin && !await access.IsMemberAsync(c.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization membership is required to start an SLA clock.");
            if (!await repo.RequestUsesServiceAsync(c.OrganizationId, c.RequestId, c.ServiceId, ct)) 
                throw new KeyNotFoundException("Request not found in organization or does not use the supplied service.");
            if (await repo.GetClockForRequestAsync(c.RequestId, ct) is not null) 
                throw new InvalidOperationException("SLA clock already exists for request.");
            var policy = await repo.GetPolicyAsync(c.OrganizationId, c.ServiceId, ct) ?? 
                throw new KeyNotFoundException("Active SLA policy not found.");
            var clock = SlaClock.Start(c.OrganizationId, c.RequestId, c.ServiceId, policy, c.StartedAtUtc ?? DateTime.UtcNow);
            await repo.AddClockAsync(clock, ct); 
            await unit.SaveChangesAsync(ct); 
            return clock.Id;
        }
    }
}
