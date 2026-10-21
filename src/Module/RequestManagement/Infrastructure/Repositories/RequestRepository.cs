using Microsoft.EntityFrameworkCore;
using RequestManagement.Application.Interfaces;
using RequestManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RequestManagement.Infrastructure.Repositories
{
    public sealed class RequestRepository(RequestManagementDbContext db) : IRequestRepository
    {
        public async Task AddAsync(ServiceRequest request, CancellationToken ct)
        {
            await db.Requests.AddAsync(request, ct);
            var payload = JsonSerializer.Serialize(new { request.Id, request.OrganizationId, request.ServiceId, request.RequestNumber, request.SubmittedByUserId, OccurredAtUtc = request.CreatedAtUtc });
            await db.OutboxMessages.AddAsync(OutboxMessage.Create("RequestSubmitted", payload, request.CreatedAtUtc), ct);
        }
        public Task<ServiceRequest?> GetForUpdateAsync(Guid id, CancellationToken ct) => db.Requests.SingleOrDefaultAsync(x => x.Id == id, ct);

        public Task<bool> IsOrganizationAdminAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [OrganizationId]={0} AND [UserId]={1} AND [Role]={2} AND [RemovedAtUtc] IS NULL", organizationId, userId, 2).AnyAsync(ct);


        public Task<bool> IsOrganizationMemberAsync(Guid organizationId, Guid userId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [OrganizationId]={0} AND [UserId]={1} AND [RemovedAtUtc] IS NULL", organizationId, userId).AnyAsync(ct);

        public Task<bool> ServiceIsActiveInOrganizationAsync(Guid serviceId, Guid organizationId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [Id] AS [Value] FROM [service_catalog].[Services] WHERE [Id]={0} AND [OrganizationId]={1} AND [IsArchived]=0", serviceId, organizationId).AnyAsync(ct);

        public Task UpdateAsync(ServiceRequest request, CancellationToken ct) 
        { 
            db.Requests.Update(request); 
            return Task.CompletedTask;
        }
    }
}
