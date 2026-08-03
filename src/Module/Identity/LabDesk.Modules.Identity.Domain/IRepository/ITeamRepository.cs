using LabDesk.Modules.Identity.Domain.Aggregates;
using LabDesk.SeedWork.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Domain.IRepository
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Team>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default);
        void Add(Team team);
        void Update(Team team);
    }
}
