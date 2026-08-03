using LabDesk.SeedWork.Application.Interfaces;
using LabDesk.SeedWork.Infrastructure.Persistence;
using LabDesk.SeedWork.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSeedWorkInfrastructure(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<DispatchDomainEventsInterceptor>();

            return services;
        }
    }
}
