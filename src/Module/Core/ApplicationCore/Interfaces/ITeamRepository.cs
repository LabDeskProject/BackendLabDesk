using DomainCore.Teams;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface ITeamRepository
    {
        Task<bool> ExistsByNameAsync(
            Guid organizationId,
            string name,
            CancellationToken cancellationToken);

        Task AddAsync(
            Team team,
            CancellationToken cancellationToken);
    }
}
