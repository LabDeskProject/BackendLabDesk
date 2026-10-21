using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RequestManagement.Application;
using RequestManagement.Application.Interfaces;
using RequestManagement.Infrastructure.Repositories;

namespace RequestManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRequestManagementModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connection)) throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        services.AddRequestManagementApplication();
        services.AddHttpContextAccessor();
        services.AddDbContext<RequestManagementDbContext>(o => o.UseSqlServer(connection, sql => sql.MigrationsAssembly(typeof(RequestManagementDbContext).Assembly.FullName)));
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IRequestQueries, RequestQueries>();
        services.AddScoped<IRequestUnitOfWork, RequestUnitOfWork>();
        services.AddScoped<IRequestNumberAllocator, RequestNumberAllocator>();
        services.AddScoped<IRequestSubmissionTransaction, RequestSubmissionTransaction>();
        services.AddScoped<IRequestUserContext, RequestUserContext>();
        return services;
    }
}
