using Assignment.Application.Contracts;
using Assignment.Application.Interfaces;
using Assignment.Domain;
using Assignment.Domain.Enum;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Infrastructure.Repositories
{
    public sealed class AssignmentRepositories(AssignmentDbContext db) : IAssignmentRepository
    {
        public Task<bool> RequestExistsAsync(Guid requestId, Guid organizationId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [Id] AS [Value] FROM [request_management].[Requests] WHERE [Id]={0} AND [OrganizationId]={1}", requestId, organizationId).AnyAsync(ct);
        public Task<bool> IsRequestInOrganizationAsync(Guid requestId, Guid organizationId, CancellationToken ct) => RequestExistsAsync(requestId, organizationId, ct);
        public async Task<Guid?> GetRequestOrganizationAsync(Guid requestId, CancellationToken ct) => await db.Database.SqlQueryRaw<Guid>("SELECT [OrganizationId] AS [Value] FROM [request_management].[Requests] WHERE [Id]={0}", requestId).FirstOrDefaultAsync(ct) is var id && id != Guid.Empty ? id : null;
        public Task<bool> AssigneeIsMemberAsync(Guid userId, Guid organizationId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [UserId]={0} AND [OrganizationId]={1} AND [RemovedAtUtc] IS NULL", userId, organizationId).AnyAsync(ct);
        public Task<bool> IsOrganizationMemberAsync(Guid userId, Guid organizationId, CancellationToken ct) => AssigneeIsMemberAsync(userId, organizationId, ct);
        public Task<bool> IsOrganizationAdminAsync(Guid userId, Guid organizationId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [UserId] AS [Value] FROM [organization_management].[OrganizationUsers] WHERE [UserId]={0} AND [OrganizationId]={1} AND [Role]=2 AND [RemovedAtUtc] IS NULL", userId, organizationId).AnyAsync(ct);
        public Task<bool> IsTeamInOrganizationAsync(Guid teamId, Guid organizationId, CancellationToken ct) => db.Database.SqlQueryRaw<Guid>("SELECT [Id] AS [Value] FROM [organization_management].[Teams] WHERE [Id]={0} AND [OrganizationId]={1} AND [IsArchived]=0", teamId, organizationId).AnyAsync(ct);
        public Task<AssignmentRecord?> GetActiveForRequestAsync(Guid requestId, CancellationToken ct) => db.Assignments.SingleOrDefaultAsync(x => x.RequestId == requestId && x.Status == AssignmentStatus.Active, ct);
        public Task<AssignmentRecord?> GetActiveForRequestAsyncById(Guid assignmentId, CancellationToken ct) => db.Assignments.SingleOrDefaultAsync(x => x.Id == assignmentId && x.Status == AssignmentStatus.Active, ct);
        public Task AddAsync(AssignmentRecord assignment, CancellationToken ct) => db.Assignments.AddAsync(assignment, ct).AsTask();
        public Task UpdateAsync(AssignmentRecord assignment, CancellationToken ct) { db.Assignments.Update(assignment); return Task.CompletedTask; }
        public async Task<IReadOnlyList<AssignmentListItem>> ListForRequestAsync(Guid requestId, CancellationToken ct) => await db.Assignments.AsNoTracking().Where(x => x.RequestId == requestId).OrderByDescending(x => x.AssignedAtUtc).Select(x => new AssignmentListItem(x.Id, x.OrganizationId, x.RequestId, x.AssignedToUserId, x.AssignedByUserId, x.TeamId, x.Note, x.Status, x.AssignedAtUtc, x.EndedAtUtc)).ToListAsync(ct);
        public async Task<IReadOnlyList<AssignmentListItem>> ListForUserAsync(Guid organizationId, Guid userId, int skip, int take, CancellationToken ct)
        {
            var c = db.Database.GetDbConnection(); var opened = c.State != System.Data.ConnectionState.Open; if (opened) await db.Database.OpenConnectionAsync(ct);
            try 
            { 
                return (await c.QueryAsync<AssignmentListItem>(new CommandDefinition("SELECT Id,OrganizationId,RequestId,AssignedToUserId,AssignedByUserId,TeamId,Note,Status,AssignedAtUtc,EndedAtUtc FROM assignment.Assignments WHERE OrganizationId=@organizationId AND AssignedToUserId=@userId ORDER BY AssignedAtUtc DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY", new { organizationId, userId, skip, take }, cancellationToken: ct))).AsList(); 
            }
            finally 
            { 
                if (opened) await db.Database.CloseConnectionAsync(); 
            }
        }
    }
}
