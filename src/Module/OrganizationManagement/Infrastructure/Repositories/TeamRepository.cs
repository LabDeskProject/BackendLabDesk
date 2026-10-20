using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace OrganizationManagement.Infrastructure.Repositories
{
    public sealed class TeamRepository(OrganizationManagementDbContext db) : ITeamRepository
    {
        public Task AddAsync(Team team, CancellationToken ct) => db.Teams.AddAsync(team, ct).AsTask();

        Task<bool> ITeamRepository.ExistsByNameAsync(Guid organizationId, string name, CancellationToken ct) => db.Teams.AnyAsync(x => x.OrganizationId == organizationId && x.Name == name.Trim() && !x.IsArchived, ct);
    }
}
