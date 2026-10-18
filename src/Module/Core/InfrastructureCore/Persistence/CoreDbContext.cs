using DomainCore.Organizations;
using DomainCore.Teams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace InfrastructureCore.Persistence
{
    public sealed class CoreDbContext : DbContext
    {
        public CoreDbContext(
            DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<OrganizationUser> OrganizationUsers => Set<OrganizationUser>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(CoreDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
