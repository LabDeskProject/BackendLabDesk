using Notification.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain
{
    public sealed class NotificationMessage
    {
        private NotificationMessage() { }
        public Guid Id { get; private set; }
        public Guid OrganizationId { get; private set; }
        public Guid RecipientUserId { get; private set; }
        public string Type { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public string Body { get; private set; } = string.Empty;
        public string? PayloadJson { get; private set; }
        public string IdempotencyKey { get; private set; } = string.Empty;
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? ReadAtUtc { get; private set; }
        public ICollection<NotificationDelivery> Deliveries { get; private set; } = new List<NotificationDelivery>();
        public static NotificationMessage Create(Guid organizationId, Guid recipientUserId, string type, string title, string body, string idempotencyKey, string? payloadJson, IEnumerable<DeliveryChannel> channels, DateTime nowUtc)
        {
            if (organizationId == Guid.Empty || recipientUserId == Guid.Empty) 
                throw new ArgumentException("Organization and recipient IDs are required.");
            if (string.IsNullOrWhiteSpace(type) || type.Length > 100) 
                throw new ArgumentException("Type is required and limited to 100 characters.");
            if (string.IsNullOrWhiteSpace(title) || title.Length > 200) 
                throw new ArgumentException("Title is required and limited to 200 characters.");
            if (string.IsNullOrWhiteSpace(body) || body.Length > 4000) 
                throw new ArgumentException("Body is required and limited to 4000 characters.");
            if (string.IsNullOrWhiteSpace(idempotencyKey) || idempotencyKey.Length > 200) 
                throw new ArgumentException("Idempotency key is required and limited to 200 characters.");
            if (payloadJson is not null) 
            { 
                try 
                { using var doc = System.Text.Json.JsonDocument.Parse(payloadJson); 
                    if (doc.RootElement.ValueKind != System.Text.Json.JsonValueKind.Object) 
                        throw new ArgumentException("Payload must be a JSON object.", nameof(payloadJson)); 
                } 
                catch (System.Text.Json.JsonException ex) 
                { 
                    throw new ArgumentException("Payload must be valid JSON.", nameof(payloadJson), ex); 
                } 
            }
            ArgumentNullException.ThrowIfNull(channels);
            var selected = channels.Distinct().ToList(); if (selected.Count == 0 || selected.Any(x => !System.Enum.IsDefined(x))) 
                throw new ArgumentException("At least one supported delivery channel is required.", nameof(channels));
            var message = new NotificationMessage { Id = Guid.NewGuid(), OrganizationId = organizationId, RecipientUserId = recipientUserId, Type = type.Trim(), Title = title.Trim(), Body = body.Trim(), IdempotencyKey = idempotencyKey.Trim(), PayloadJson = payloadJson, CreatedAtUtc = nowUtc };
            foreach (var channel in selected) message.Deliveries.Add(NotificationDelivery.Create(message.Id, channel, nowUtc));
            return message;
        }
    }
}
