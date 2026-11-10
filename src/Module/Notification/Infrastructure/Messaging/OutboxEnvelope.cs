using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Messaging
{
    public sealed record OutboxEnvelope(
    Guid EventId,
    string EventType,
    string SourceSchema, // Source schema of the event payload
    DateTime OccurredAtUtc,
    string Payload // Json serialized payload
    );
}
