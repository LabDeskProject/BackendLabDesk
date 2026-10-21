namespace WorkflowApproval.Domain;
public sealed class ApprovalOutboxMessage
{
    private ApprovalOutboxMessage() { }
    public Guid Id { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Payload { get; private set; } = string.Empty;
    public DateTime OccurredAtUtc { get; private set; }
    public DateTime? ProcessedAtUtc { get; private set; }
    public DateTime? ClaimedAtUtc { get; private set; }
    public int AttemptCount { get; private set; }
    public string? LastError { get; private set; }
    public static ApprovalOutboxMessage Create(string type, string payload, DateTime at) => new() { Id = Guid.NewGuid(), Type = type, Payload = payload, OccurredAtUtc = at };
}
