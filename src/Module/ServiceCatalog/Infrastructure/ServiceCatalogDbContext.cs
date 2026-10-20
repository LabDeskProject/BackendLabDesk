using Microsoft.EntityFrameworkCore;
using ServiceCatalog.Domain;
using ServiceCatalog.Infrastructure.Migrations;

namespace ServiceCatalog.Infrastructure;

public sealed class ServiceCatalogDbContext(DbContextOptions<ServiceCatalogDbContext> options) : DbContext(options)
{
    public DbSet<ServiceDefinition> Services => Set<ServiceDefinition>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServiceCatalogDbContext).Assembly);
        modelBuilder.HasDefaultSchema("service_catalog");
        base.OnModelCreating(modelBuilder);
    }
}
