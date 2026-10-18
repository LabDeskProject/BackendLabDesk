using DomainCore.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<bool> ExistsByIdAsync(
            Guid organizationId,
            CancellationToken cancellationToken);
        Task<bool> ExistsBySlugAsync(
            string slug,
            CancellationToken cancellationToken);

        Task AddAsync(
            Organization organization,
            CancellationToken cancellationToken);
        Task AddMemberAsync(
        OrganizationUser organizationUser,
        CancellationToken cancellationToken);
    }
}
