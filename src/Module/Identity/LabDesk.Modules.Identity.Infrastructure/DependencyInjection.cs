using LabDesk.Modules.Identity.Application.Interfaces;
using LabDesk.Modules.Identity.Domain.IRepository;
using LabDesk.Modules.Identity.Infrastructure.Persistence;
using LabDesk.Modules.Identity.Infrastructure.Persistence.Repositories;
using LabDesk.SeedWork.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IIdentityDbContext>(provider => provider.GetRequiredService<IdentityDbContext>());
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<IdentityDbContext>());

            // 3. Register Repositories
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
