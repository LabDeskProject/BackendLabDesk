using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RequestManagement.Infrastructure;

public sealed class RequestManagementDesignTimeFactory : IDesignTimeDbContextFactory<RequestManagementDbContext>
{
    public RequestManagementDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("LABDESK_CONNECTION_STRING") ?? "Server=(localdb)\\MSSQLLocalDB;Database=LabDeskMigrationDesign;Trusted_Connection=True;TrustServerCertificate=True";
        return new RequestManagementDbContext(new DbContextOptionsBuilder<RequestManagementDbContext>().UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(RequestManagementDbContext).Assembly.FullName)).Options);
    }
}
