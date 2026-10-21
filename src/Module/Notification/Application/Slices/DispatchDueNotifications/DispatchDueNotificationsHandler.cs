using MediatR;
using Notification.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Slices.DispatchDueNotifications
{
    public sealed class DispatchDueNotificationsHandler(INotificationRepository repo, INotificationSender sender) : IRequestHandler<DispatchDueNotificationsCommand, int>
    {
        public async Task<int> Handle(DispatchDueNotificationsCommand c, CancellationToken ct)
        {
            var due = await repo.ClaimDueDeliveriesAsync(DateTime.UtcNow, Math.Clamp(c.BatchSize, 1, 500), ct); var sent = 0;
            foreach (var (delivery, message) in due)
            {
                try { await sender.SendAsync(delivery.Channel, message, ct); delivery.MarkSent(DateTime.UtcNow); sent++; }
                catch (Exception ex) when (ex is not OperationCanceledException) { delivery.MarkFailed(ex.Message, DateTime.UtcNow.AddMinutes(Math.Min(60, Math.Pow(2, Math.Min(delivery.AttemptCount, 6))))); }
            }
            if (due.Count > 0) await repo.SaveChangesAsync(ct); return sent;
        }
    }
}
