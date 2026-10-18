using Application.Interface;
using Domain.User;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public sealed class LoginRepository : ILoginRepository
    {
        private readonly IdentityAccessDbContext _dbContext;
        public LoginRepository(
            IdentityAccessDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken) =>
            _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
}
