using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrganizationManagement.Infrastructure.Repositories;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain.Interfaces;

namespace OrganizationManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrganizationManagementInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString)) throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        services.AddDbContext<OrganizationManagementDbContext>(options => options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(OrganizationManagementDbContext).Assembly.FullName)));
        services.AddScoped<IOrganizationManagementUnitOfWork, OrganizationManagementUnitOfWork>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IOrganizationUserRepository, OrganizationUserRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IServiceQueries, ServiceQueries>();
        return services;
    }
}
