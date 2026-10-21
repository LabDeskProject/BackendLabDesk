using Notification.Domain;
using Notification.Domain.Enum;
namespace Notification.Domain.Tests;
public sealed class NotificationTests
{
    [Fact]
    public void Create_generates_channel_delivery_rows_and_preserves_idempotency_key()
    {
        var n = NotificationMessage.Create(Guid.NewGuid(), Guid.NewGuid(), "RequestUpdate", "Title", "Body", "evt-123", "{\"requestId\":\"x\"}", [DeliveryChannel.InApp, DeliveryChannel.Email], DateTime.UtcNow);
        Assert.Equal("evt-123", n.IdempotencyKey); Assert.Equal(2, n.Deliveries.Count); Assert.All(n.Deliveries, x => Assert.Equal(DeliveryStatus.Pending, x.Status));
    }
    [Fact]
    public void Create_rejects_invalid_payload_and_no_channels()
    {
        Assert.ThrowsAny<Exception>(() => NotificationMessage.Create(Guid.NewGuid(), Guid.NewGuid(), "type", "title", "body", "key", "[]", [DeliveryChannel.InApp], DateTime.UtcNow));
        Assert.Throws<ArgumentException>(() => NotificationMessage.Create(Guid.NewGuid(), Guid.NewGuid(), "type", "title", "body", "key", null, [], DateTime.UtcNow));
        Assert.Throws<ArgumentNullException>(() => NotificationMessage.Create(Guid.NewGuid(), Guid.NewGuid(), "type", "title", "body", "key", null, null!, DateTime.UtcNow));
        Assert.Throws<ArgumentException>(() => NotificationMessage.Create(Guid.NewGuid(), Guid.NewGuid(), "type", "title", "body", "key", "{invalid", [DeliveryChannel.InApp], DateTime.UtcNow));
    }
    [Fact]
    public void Delivery_tracks_failure_retry_then_success()
    {
        var at = DateTime.UtcNow; var delivery = NotificationDelivery.Create(Guid.NewGuid(), DeliveryChannel.Email, at);
        delivery.MarkFailed("provider unavailable", at.AddMinutes(1)); Assert.Equal(1, delivery.AttemptCount); Assert.Equal(DeliveryStatus.Failed, delivery.Status);
        delivery.ScheduleRetry(at.AddMinutes(1)); Assert.Equal(DeliveryStatus.Pending, delivery.Status);
        delivery.MarkSent(at.AddMinutes(2)); Assert.Equal(DeliveryStatus.Sent, delivery.Status); Assert.Equal(2, delivery.AttemptCount);
        Assert.Throws<InvalidOperationException>(() => delivery.MarkFailed("late error", at.AddMinutes(3)));
    }
}
