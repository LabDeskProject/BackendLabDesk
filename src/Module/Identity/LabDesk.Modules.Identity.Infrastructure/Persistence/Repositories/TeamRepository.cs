using LabDesk.Modules.Identity.Domain.Aggregates;
using LabDesk.Modules.Identity.Domain.IRepository;
using LabDesk.SeedWork.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Infrastructure.Persistence.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IdentityDbContext _context;
        public TeamRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Team team)
        {
            _context.Add(team);
        }

        public async Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Teams.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Team>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default)
        {
            return await _context.Teams
            .Where(x => x.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
        }

        public void Update(Team team)
        {
            _context.Teams.Update(team);
        }
    }
}
