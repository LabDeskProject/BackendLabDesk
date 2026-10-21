using Dapper;
using Microsoft.EntityFrameworkCore;
using SLA.Application.Contracts;
using SLA.Application.Interfaces;
using SLA.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Infrastructure.Repositories
{
    public sealed class SlaRepository(SlaDbContext db) : ISlaRepository
    {
        public Task<bool> ServiceIsActiveAsync(Guid organizationId, Guid serviceId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [Id] AS [Value] FROM [service_catalog].[Services] WHERE [Id]={0} AND [OrganizationId]={1} AND [IsArchived]=0", serviceId, organizationId).AnyAsync(ct);
        public Task<bool> RequestUsesServiceAsync(Guid organizationId, Guid requestId, Guid serviceId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [Id] AS [Value] FROM [request_management].[Requests] WHERE [Id]={0} AND [OrganizationId]={1} AND [ServiceId]={2}", requestId, organizationId, serviceId).AnyAsync(ct);
        public Task<SlaPolicy?> GetPolicyAsync(Guid organizationId, Guid serviceId, CancellationToken ct) => db.Policies.SingleOrDefaultAsync(x => x.OrganizationId == organizationId && x.ServiceId == serviceId && !x.IsArchived, ct);
        public Task<SlaPolicy?> GetPolicyByIdAsync(Guid id, CancellationToken ct) => db.Policies.SingleOrDefaultAsync(x => x.Id == id && !x.IsArchived, ct);
        public Task<SlaClock?> GetClockForRequestAsync(Guid requestId, CancellationToken ct) => db.Clocks.SingleOrDefaultAsync(x => x.RequestId == requestId, ct);
        public Task<SlaClock?> GetClockByIdAsync(Guid id, CancellationToken ct) => db.Clocks.SingleOrDefaultAsync(x => x.Id == id, ct);
        public Task AddPolicyAsync(SlaPolicy policy, CancellationToken ct) => db.Policies.AddAsync(policy, ct).AsTask();
        public Task AddClockAsync(SlaClock clock, CancellationToken ct) => db.Clocks.AddAsync(clock, ct).AsTask();
        public async Task<IReadOnlyList<SlaPolicyItem>> ListPoliciesAsync(Guid organizationId, CancellationToken ct) => await db.Policies.AsNoTracking().Where(x => x.OrganizationId == organizationId).Select(x => new SlaPolicyItem(x.Id, x.OrganizationId, x.ServiceId, x.ResponseMinutes, x.ResolutionMinutes, x.IsArchived, x.CreatedAtUtc)).ToListAsync(ct);
        public async Task<IReadOnlyList<SlaClockItem>> ListBreachesAsync(Guid organizationId, int skip, int take, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open;
            if (opened) await db.Database.OpenConnectionAsync(ct);
            try { return (await c.QueryAsync<SlaClockItem>(new CommandDefinition("SELECT Id, OrganizationId, RequestId, ServiceId, StartedAtUtc, ResponseDueAtUtc, ResolutionDueAtUtc, FirstResponseAtUtc, ResolvedAtUtc, Status FROM sla.Clocks WHERE OrganizationId=@organizationId AND Status IN (2,3) ORDER BY ResolutionDueAtUtc OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY", new { organizationId, skip, take }, cancellationToken: ct))).AsList(); }
            finally { if (opened) await db.Database.CloseConnectionAsync(); }
        }
    }
}
