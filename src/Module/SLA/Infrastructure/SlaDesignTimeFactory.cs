using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace SLA.Infrastructure;
public sealed class SlaDesignTimeFactory : IDesignTimeDbContextFactory<SlaDbContext>
{
    public SlaDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("LABDESK_CONNECTION_STRING") ?? "Server=(localdb)\\MSSQLLocalDB;Database=LabDeskMigrationDesign;Trusted_Connection=True;TrustServerCertificate=True";
        return new SlaDbContext(new DbContextOptionsBuilder<SlaDbContext>().UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(SlaDbContext).Assembly.FullName)).Options);
    }
}
