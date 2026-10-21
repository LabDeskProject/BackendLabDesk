using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Notification.Infrastructure;
public sealed class NotificationDesignTimeFactory : IDesignTimeDbContextFactory<NotificationDbContext>
{
    public NotificationDbContext CreateDbContext(string[] args) { var cs = Environment.GetEnvironmentVariable("LABDESK_CONNECTION_STRING") ?? "Server=(localdb)\\MSSQLLocalDB;Database=LabDeskMigrationDesign;Trusted_Connection=True;TrustServerCertificate=True"; return new NotificationDbContext(new DbContextOptionsBuilder<NotificationDbContext>().UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(NotificationDbContext).Assembly.FullName)).Options); }
}
