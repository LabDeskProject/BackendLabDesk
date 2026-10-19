using Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface
{
    public interface ILoginRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
