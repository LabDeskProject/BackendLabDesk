using Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface
{
    public interface IUserRepository
    {
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
        void Add(User user);
        Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task SetGlobalAdminAsync(Guid userId, bool isGlobalAdmin, CancellationToken cancellationToken);
        Task BootstrapGlobalAdminAsync(string email, CancellationToken cancellationToken);
    }
}
