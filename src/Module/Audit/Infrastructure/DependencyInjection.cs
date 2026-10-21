using Audit.Application;
using Audit.Application.Interfaces;
using Audit.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Audit.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddAuditModule(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("DefaultConnection"); 
        if (string.IsNullOrWhiteSpace(cs)) 
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        services.AddAuditApplication(); 
        services.AddDbContext<AuditDbContext>(o => o.UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(AuditDbContext).Assembly.FullName)));
        services.AddScoped<IAuditIdempotentWriter, AuditIdempotentWriter>();
        services.AddScoped<IAuditWriter>(sp => sp.GetRequiredService<IAuditIdempotentWriter>());
        services.AddScoped<IAuditQueries, AuditQueries>(); 
        services.AddScoped<IAuditAccess, AuditAccess>(); 
        services.AddHttpContextAccessor(); services.AddScoped<IAuditUserContext, AuditUserContext>(); 
        return services;
    }
}
