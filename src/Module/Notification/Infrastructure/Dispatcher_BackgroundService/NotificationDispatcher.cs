using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.Application;
using Notification.Application.Slices.DispatchDueNotifications;
namespace Notification.Infrastructure.Dispatcher_BackgroundService;
public sealed class NotificationDispatcher(IServiceScopeFactory scopes, ILogger<NotificationDispatcher> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopes.CreateAsyncScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                var count = await sender.Send(new DispatchDueNotificationsCommand(100), stoppingToken);
                if (count == 0) await timer.WaitForNextTickAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }
            catch (Exception ex) { logger.LogError(ex, "Notification dispatch cycle failed"); await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); }
        }
    }
}
