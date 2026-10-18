using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IOrganizationUserRepository
    {
        Task<bool> ExistsAsync(
            Guid organizationId,
            Guid userId,
            CancellationToken cancellationToken);
    }
}
