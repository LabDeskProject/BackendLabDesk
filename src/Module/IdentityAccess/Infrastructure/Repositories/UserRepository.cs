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

        public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken) =>
            _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

        public async Task BootstrapGlobalAdminAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email)) 
                throw new ArgumentException("Bootstrap admin email is required.", nameof(email));
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken);

            var administratorExists = await _dbContext.Users.AnyAsync(x => x.IsGlobalAdmin && x.IsActive, cancellationToken);
            if (administratorExists) throw new InvalidOperationException("Global administrator bootstrap has already been completed.");

            var normalized = email.Trim().ToLowerInvariant();
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == normalized && x.IsActive, cancellationToken)
                ?? throw new KeyNotFoundException("Active bootstrap user not found. Register the designated account before bootstrap.");
            if (!user.TryBootstrapGlobalAdmin(normalized)) 
                throw new InvalidOperationException("Global administrator bootstrap failed.");
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task SetGlobalAdminAsync(Guid userId, bool isGlobalAdmin, CancellationToken cancellationToken)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken);
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken)
                ?? throw new KeyNotFoundException("User not found.");

            if (user.IsGlobalAdmin && !isGlobalAdmin && user.IsActive)
            {
                var otherAdministrators = await _dbContext.Users.CountAsync(x => x.IsGlobalAdmin && x.IsActive && x.Id != userId, cancellationToken);
                if (otherAdministrators == 0) throw new InvalidOperationException("The last active global administrator cannot be demoted.");
            }
            user.SetGlobalAdmin(isGlobalAdmin);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }
}
