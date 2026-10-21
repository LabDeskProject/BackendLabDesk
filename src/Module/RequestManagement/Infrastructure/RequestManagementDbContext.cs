using Microsoft.EntityFrameworkCore;
using RequestManagement.Domain;

namespace RequestManagement.Infrastructure;

public sealed class RequestManagementDbContext(DbContextOptions<RequestManagementDbContext> options) : DbContext(options)
{
    public DbSet<ServiceRequest> Requests => Set<ServiceRequest>();
    public DbSet<RequestNumberSequence> RequestNumberSequences => Set<RequestNumberSequence>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RequestManagementDbContext).Assembly);
        modelBuilder.HasDefaultSchema("request_management");
        base.OnModelCreating(modelBuilder);
    }
}
