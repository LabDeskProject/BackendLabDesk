using RequestManagement.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Interfaces
{
    public interface IRequestQueries
    {
        Task<RequestListItem?> GetAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<RequestListItem>> ListAsync(Guid organizationId, Guid userId, bool isAdmin, int skip, int take, CancellationToken ct);
    }
}
