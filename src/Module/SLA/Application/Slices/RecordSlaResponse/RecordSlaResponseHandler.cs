using BuildingBlocks.Application;
using MediatR;
using SLA.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Slices.RecordSlaResponse
{
    public sealed class RecordSlaResponseHandler(ISlaRepository repo, ISlaUnitOfWork unit, ISlaAccess access, IUserContext user, ISlaUserContext global) : IRequestHandler<RecordSlaResponseCommand>
    {
        public async Task Handle(RecordSlaResponseCommand c, CancellationToken ct)
        {
            var clock = await repo.GetClockByIdAsync(c.ClockId, ct) ?? 
                throw new KeyNotFoundException("SLA clock not found."); 
            if (!global.IsAdmin && !await access.IsMemberAsync(clock.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization membership is required."); 
            clock.RecordResponse(c.RespondedAtUtc ?? DateTime.UtcNow); 
            await unit.SaveChangesAsync(ct);
        }
    }
}
