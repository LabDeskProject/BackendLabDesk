using Domain.User;
using Domain.RefreshTokens;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Infrastructure.Persistence
{
    public sealed class IdentityAccessDbContext : DbContext
    {
        public IdentityAccessDbContext(
            DbContextOptions<IdentityAccessDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(IdentityAccessDbContext).Assembly);
        }
    }
}
