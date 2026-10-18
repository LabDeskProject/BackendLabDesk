using InfrastructureCore;
using ApplicationCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace APICore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddCoreApplication();

            services.AddCoreInfrastructure(configuration);

            return services;
        }
    }
}
