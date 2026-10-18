using ApplicationCore.Interfaces;
using DomainCore.Interfaces;
using InfrastructureCore.Persistence;
using InfrastructureCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' is not configured.");
            }

            services.AddDbContext<CoreDbContext>(
                options =>
                    options.UseSqlServer(connectionString));

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();

            services.AddScoped<IOrganizationUserRepository, OrganizationUserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IUnitOfWorkCore, CoreUnitOfWork>();

            return services;
        }
    }
}
