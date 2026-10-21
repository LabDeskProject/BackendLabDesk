using Audit.Domain;
using Microsoft.EntityFrameworkCore;
namespace Audit.Infrastructure;
public sealed class AuditDbContext(DbContextOptions<AuditDbContext> options) : DbContext(options)
{
    public DbSet<AuditEntry> Entries => Set<AuditEntry>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuditDbContext).Assembly);
        modelBuilder.HasDefaultSchema("audit");
        base.OnModelCreating(modelBuilder);
    }
    public override int SaveChanges() => throw new NotSupportedException("Audit entries are append-only.");
    public override int SaveChanges(bool acceptAllChangesOnSuccess) => throw new NotSupportedException("Audit entries are append-only.");
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => throw new NotSupportedException("Audit entries are append-only.");
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) => throw new NotSupportedException("Audit entries are append-only.");
}
