namespace Audit.Application.Contracts;
public sealed record AuditEntryItem(Guid Id, Guid OrganizationId, Guid? ActorUserId, string Action, string EntityType, string EntityId, string? CorrelationId, string? DetailsJson, DateTime OccurredAtUtc);
