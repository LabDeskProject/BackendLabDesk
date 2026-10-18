using IdentityAccess.API.Mapping;
using Application;
using BuildingBlocks.Application;
using IdentityAccess.API.Authentication;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAccess.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityAccessModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityAccessApplication();

            services.AddIdentityAccessInfrastructure(configuration);

            services.AddAutoMapper(
                cfg => { },
                typeof(RegisterProfile).Assembly);

            services.AddHttpContextAccessor();

            services.AddScoped<IUserContext, HttpUserContext>();
            return services;
        }
    }
}
