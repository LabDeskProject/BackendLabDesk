using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrganizationManagement.Infrastructure;

public sealed class OrganizationManagementDesignTimeFactory : IDesignTimeDbContextFactory<OrganizationManagementDbContext>
{
    public OrganizationManagementDbContext CreateDbContext(string[] args)
    {
        var connection = Environment.GetEnvironmentVariable("LABDESK_CONNECTION_STRING")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=LabDeskMigrationDesign;Trusted_Connection=True;TrustServerCertificate=True";
        var options = new DbContextOptionsBuilder<OrganizationManagementDbContext>().UseSqlServer(connection, sql => sql.MigrationsAssembly(typeof(OrganizationManagementDbContext).Assembly.FullName)).Options;
        return new OrganizationManagementDbContext(options);
    }
}
