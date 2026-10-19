using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrganizationManagement.Application;
using OrganizationManagement.Infrastructure;

namespace OrganizationManagement.API;

public static class DependencyInjection
{
    public static IServiceCollection AddOrganizationManagementModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOrganizationManagementApplication();
        services.AddOrganizationManagementInfrastructure(configuration);
        return services;
    }
}
