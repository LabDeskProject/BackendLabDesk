using Assignment.Domain;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Infrastructure;

public sealed class AssignmentDbContext(DbContextOptions<AssignmentDbContext> options) : DbContext(options)
{
    public DbSet<AssignmentRecord> Assignments => Set<AssignmentRecord>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssignmentDbContext).Assembly);
        modelBuilder.HasDefaultSchema("assignment");
        base.OnModelCreating(modelBuilder);
    }
}
