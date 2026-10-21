using Microsoft.EntityFrameworkCore;
using Notification.Domain;
namespace Notification.Infrastructure;
public sealed class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : DbContext(options)
{
    public DbSet<NotificationMessage> Notifications => Set<NotificationMessage>();
    public DbSet<NotificationDelivery> Deliveries => Set<NotificationDelivery>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
        modelBuilder.HasDefaultSchema("notification");
        base.OnModelCreating(modelBuilder);
    }
}
