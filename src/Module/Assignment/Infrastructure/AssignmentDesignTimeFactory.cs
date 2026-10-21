using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Assignment.Infrastructure;
public sealed class AssignmentDesignTimeFactory : IDesignTimeDbContextFactory<AssignmentDbContext>
{
    public AssignmentDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("LABDESK_CONNECTION_STRING") ?? "Server=(localdb)\\MSSQLLocalDB;Database=LabDeskMigrationDesign;Trusted_Connection=True;TrustServerCertificate=True";
        return new AssignmentDbContext(new DbContextOptionsBuilder<AssignmentDbContext>().UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(AssignmentDbContext).Assembly.FullName)).Options);
    }
}
