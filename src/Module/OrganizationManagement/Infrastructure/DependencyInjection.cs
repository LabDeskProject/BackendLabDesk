using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrganizationManagement.Domain;
using OrganizationManagement.Infrastructure.Repositories;
using OrganizationManagement.Application.Interfaces;

namespace OrganizationManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrganizationManagementInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString)) throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        services.AddDbContext<OrganizationManagementDbContext>(options => options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(OrganizationManagementDbContext).Assembly.FullName)));
        services.AddScoped<IOrganizationManagementUnitOfWork, OrganizationManagementUnitOfWork>();
        services.AddScoped<OrganizationManagementRepositories>();
        services.AddScoped<IOrganizationRepository>(sp => sp.GetRequiredService<OrganizationManagementRepositories>());
        services.AddScoped<IOrganizationUserRepository>(sp => sp.GetRequiredService<OrganizationManagementRepositories>());
        services.AddScoped<ITeamRepository>(sp => sp.GetRequiredService<OrganizationManagementRepositories>());
        services.AddScoped<IServiceRepository>(sp => sp.GetRequiredService<OrganizationManagementRepositories>());
        services.AddScoped<IServiceQueries>(sp => sp.GetRequiredService<OrganizationManagementRepositories>());
        return services;
    }
}
