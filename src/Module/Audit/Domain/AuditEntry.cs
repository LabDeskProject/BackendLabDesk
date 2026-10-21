namespace Audit.Domain;

public sealed class AuditEntry
{
    private AuditEntry() { }
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid? ActorUserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string EntityType { get; private set; } = string.Empty;
    public string EntityId { get; private set; } = string.Empty;
    public string? CorrelationId { get; private set; }
    public string? DetailsJson { get; private set; }
    public DateTime OccurredAtUtc { get; private set; }
    public static AuditEntry Create(Guid organizationId, Guid? actorUserId, string action, string entityType, string entityId, string? correlationId, string? detailsJson, DateTime occurredAtUtc)
    {
        if (organizationId == Guid.Empty) 
            throw new ArgumentException("Organization is required.", nameof(organizationId));
        if (actorUserId == Guid.Empty) throw new ArgumentException("Actor ID cannot be empty.", nameof(actorUserId));
        if (string.IsNullOrWhiteSpace(action) || action.Length > 100) 
            throw new ArgumentException("Action is required and limited to 100 characters.", nameof(action));
        if (string.IsNullOrWhiteSpace(entityType) || entityType.Length > 150) 
            throw new ArgumentException("Entity type is required and limited to 150 characters.", nameof(entityType));
        if (string.IsNullOrWhiteSpace(entityId) || entityId.Length > 100) 
            throw new ArgumentException("Entity ID is required and limited to 100 characters.", nameof(entityId));
        if (!string.IsNullOrWhiteSpace(detailsJson))
        {
            using var doc = System.Text.Json.JsonDocument.Parse(detailsJson);
            if (doc.RootElement.ValueKind != System.Text.Json.JsonValueKind.Object) 
                throw new ArgumentException("Audit details must be a JSON object.", nameof(detailsJson));
        }
        if (correlationId?.Length > 100) 
            throw new ArgumentException("Correlation ID is too long.", nameof(correlationId));
        return new AuditEntry 
        { 
            Id = Guid.NewGuid(), 
            OrganizationId = organizationId, 
            ActorUserId = actorUserId, 
            Action = action.Trim(), 
            EntityType = entityType.Trim(), 
            EntityId = entityId.Trim(), 
            CorrelationId = correlationId?.Trim(), 
            DetailsJson = detailsJson, 
            OccurredAtUtc = occurredAtUtc 
        };
    }
}
