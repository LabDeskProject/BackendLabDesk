using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceCatalog.Application;
using ServiceCatalog.Infrastructure.Repositories;

namespace ServiceCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceCatalogInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connection)) throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        services.AddDbContext<ServiceCatalogDbContext>(o => o.UseSqlServer(connection, sql => sql.MigrationsAssembly(typeof(ServiceCatalogDbContext).Assembly.FullName)));
        services.AddScoped<IServiceCatalogRepository, ServiceCatalogRepository>();
        services.AddScoped<IServiceCatalogQueries, ServiceCatalogQueries>();
        services.AddScoped<IServiceCatalogUnitOfWork, ServiceCatalogUnitOfWork>();
        return services;
    }
    public static IServiceCollection AddServiceCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceCatalogApplication();
        services.AddServiceCatalogInfrastructure(configuration);
        return services;
    }
}
