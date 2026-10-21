using Notification.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain
{
    public sealed class NotificationDelivery
    {
        private NotificationDelivery() { }
        public Guid Id { get; private set; }
        public Guid NotificationId { get; private set; }
        public NotificationMessage? Notification { get; private set; }
        public DeliveryChannel Channel { get; private set; }
        public DeliveryStatus Status { get; private set; }
        public int AttemptCount { get; private set; }
        public DateTime NextAttemptAtUtc { get; private set; }
        public DateTime? SentAtUtc { get; private set; }
        public DateTime? ClaimedAtUtc { get; private set; }
        public string? LastError { get; private set; }
        public static NotificationDelivery Create(Guid notificationId, DeliveryChannel channel, DateTime nowUtc) => new() { Id = Guid.NewGuid(), NotificationId = notificationId, Channel = channel, Status = DeliveryStatus.Pending, NextAttemptAtUtc = nowUtc };
        public bool TryClaim(DateTime nowUtc, TimeSpan lease) 
        { 
            if (Status == DeliveryStatus.Sent || (Status == DeliveryStatus.Processing && ClaimedAtUtc is { } held && held > nowUtc.Subtract(lease)) || NextAttemptAtUtc > nowUtc) 
                return false; Status = DeliveryStatus.Processing; ClaimedAtUtc = nowUtc; 
            return true;
        }
        public void MarkSent(DateTime atUtc) 
        { 
            if (Status == DeliveryStatus.Sent)
                return; Status = DeliveryStatus.Sent; SentAtUtc = atUtc; ClaimedAtUtc = null; LastError = null; AttemptCount++; 
        }
        public void MarkFailed(string error, DateTime retryAtUtc) 
        { 
            if (Status == DeliveryStatus.Sent) 
                throw new InvalidOperationException("Sent delivery cannot be marked failed."); 
            Status = DeliveryStatus.Failed; LastError = error.Length > 1000 ? error[..1000] : error; 
            NextAttemptAtUtc = retryAtUtc; ClaimedAtUtc = null; AttemptCount++;
        }
        public void ScheduleRetry(DateTime retryAtUtc) 
        { if (Status == DeliveryStatus.Sent) 
                throw new InvalidOperationException("Sent delivery cannot be retried."); 
            Status = DeliveryStatus.Pending; NextAttemptAtUtc = retryAtUtc; ClaimedAtUtc = null; 
        }
    }
}
