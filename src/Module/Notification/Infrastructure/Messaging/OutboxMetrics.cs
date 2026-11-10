using Prometheus;

namespace Notification.Infrastructure.Messaging;

public static class OutboxMetrics
{
    public static readonly Counter Published = Metrics.CreateCounter(
        "labdesk_outbox_published_total", "Outbox events confirmed by RabbitMQ.", new CounterConfiguration { LabelNames = ["event_type"] });
    public static readonly Counter PublishFailures = Metrics.CreateCounter(
        "labdesk_outbox_publish_failures_total", "Outbox event publish attempts that failed.", new CounterConfiguration { LabelNames = ["event_type"] });
    public static readonly Gauge Pending = Metrics.CreateGauge(
        "labdesk_outbox_pending", "Number of rows claimed in the latest outbox batch.");
}
