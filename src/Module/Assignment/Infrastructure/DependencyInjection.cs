using Assignment.Application;
using Assignment.Application.Interfaces;
using Assignment.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Assignment.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddAssignmentModule(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(cs)) throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        services.AddAssignmentApplication(); services.AddHttpContextAccessor();
        services.AddDbContext<AssignmentDbContext>(o => o.UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(AssignmentDbContext).Assembly.FullName)));
        services.AddScoped<IAssignmentRepository, AssignmentRepositories>();
        services.AddScoped<IAssignmentUnitOfWork, AssignmentUnitOfWork>();
        services.AddScoped<IAssignmentUserContext, AssignmentUserContext>(); 
        return services;
    }
}
