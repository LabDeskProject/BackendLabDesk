using Microsoft.EntityFrameworkCore;
using SLA.Domain;
namespace SLA.Infrastructure;
public sealed class SlaDbContext(DbContextOptions<SlaDbContext> options) : DbContext(options)
{
    public DbSet<SlaPolicy> Policies => Set<SlaPolicy>();
    public DbSet<SlaClock> Clocks => Set<SlaClock>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SlaDbContext).Assembly);
        modelBuilder.HasDefaultSchema("sla");
        base.OnModelCreating(modelBuilder);
    }
}
