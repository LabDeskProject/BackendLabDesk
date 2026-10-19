using Microsoft.EntityFrameworkCore;
using DomainCore.Organizations;
using DomainCore.Teams;
using DomainCore.Service;

namespace OrganizationManagement.Infrastructure;

public sealed class OrganizationManagementDbContext(DbContextOptions<OrganizationManagementDbContext> options) : DbContext(options)
{
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<OrganizationUser> OrganizationUsers => Set<OrganizationUser>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Service> Services => Set<Service>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrganizationManagementDbContext).Assembly);
        modelBuilder.HasDefaultSchema("organization_management");
        base.OnModelCreating(modelBuilder);
    }
}
