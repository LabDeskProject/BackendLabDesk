using Application.Interface;
using Domain.User;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public sealed class UserRepository
    : IUserRepository
    {
        private readonly IdentityAccessDbContext _dbContext;

        public UserRepository(
            IdentityAccessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(
                    x => x.Email == email,
                    cancellationToken);
        }

        public void Add(User user)
        {
            _dbContext.Users.Add(user);
        }
    }
}
