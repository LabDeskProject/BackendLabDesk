using Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityAccessApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(
                    Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(
                    typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }
}
