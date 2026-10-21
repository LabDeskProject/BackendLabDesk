using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Audit.Infrastructure;
public sealed class AuditDesignTimeFactory : IDesignTimeDbContextFactory<AuditDbContext>
{
    public AuditDbContext CreateDbContext(string[] args) { var cs = Environment.GetEnvironmentVariable("LABDESK_CONNECTION_STRING") ?? "Server=(localdb)\\MSSQLLocalDB;Database=LabDeskMigrationDesign;Trusted_Connection=True;TrustServerCertificate=True"; return new AuditDbContext(new DbContextOptionsBuilder<AuditDbContext>().UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(AuditDbContext).Assembly.FullName)).Options); }
}
