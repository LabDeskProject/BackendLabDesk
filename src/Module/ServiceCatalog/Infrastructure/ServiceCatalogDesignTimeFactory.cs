using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ServiceCatalog.Infrastructure;

public sealed class ServiceCatalogDesignTimeFactory : IDesignTimeDbContextFactory<ServiceCatalogDbContext>
{
    public ServiceCatalogDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("LABDESK_CONNECTION_STRING") ?? "Server=(localdb)\\MSSQLLocalDB;Database=LabDeskMigrationDesign;Trusted_Connection=True;TrustServerCertificate=True";
        return new ServiceCatalogDbContext(new DbContextOptionsBuilder<ServiceCatalogDbContext>().UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(ServiceCatalogDbContext).Assembly.FullName)).Options);
    }
}
