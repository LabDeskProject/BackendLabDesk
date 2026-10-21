using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SLA.Application;
using SLA.Application.Contracts;
using SLA.Application.Interfaces;
using SLA.Infrastructure.Repositories;
namespace SLA.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddSlaModule(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("DefaultConnection"); 
        if (string.IsNullOrWhiteSpace(cs)) 
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        services.AddSlaApplication(); 
        services.AddDbContext<SlaDbContext>(o => o.UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(SlaDbContext).Assembly.FullName)));
        services.AddScoped<ISlaRepository, SlaRepository>(); 
        services.AddScoped<ISlaUnitOfWork, SlaUnitOfWork>(); 
        services.AddScoped<ISlaAccess, SlaAccess>(); 
        services.AddHttpContextAccessor(); 
        services.AddScoped<ISlaUserContext, SlaUserContext>(); 
        return services;
    }
}
