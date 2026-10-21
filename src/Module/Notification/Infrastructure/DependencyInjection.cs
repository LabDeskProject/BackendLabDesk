using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Dispatcher_BackgroundService;
using Notification.Infrastructure.Repositories;
using Notification.Infrastructure.Sender;
namespace Notification.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("DefaultConnection"); 
        if (string.IsNullOrWhiteSpace(cs)) 
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        services.AddNotificationApplication(); 
        services.AddScoped<ModuleOutboxWorker>(); 
        services.AddHostedService<NotificationDispatcher>(); 
        services.AddHostedService<ModuleOutboxDispatcher>(); 
        services.AddHttpContextAccessor(); 
        services.AddScoped<INotificationUserContext, NotificationUserContext>(); 
        services.AddDbContext<NotificationDbContext>(o => o.UseSqlServer(cs, sql => sql.MigrationsAssembly(typeof(NotificationDbContext).Assembly.FullName)));
        services.AddScoped<INotificationRepository, NotificationRepositories>(); 
        services.AddScoped<INotificationSender, LoggingNotificationSender>(); 
        return services;
    }
}
