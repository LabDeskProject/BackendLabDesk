using LabDesk.Modules.Identity.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Module.Access.Application
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync (string email, CancellationToken cancellationToken);
        Task AddAsync (User user, CancellationToken cancellationToken);
    }
}
