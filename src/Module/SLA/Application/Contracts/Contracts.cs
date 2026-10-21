using SLA.Domain;
using SLA.Domain.Enum;
namespace SLA.Application.Contracts;
public sealed record SlaPolicyItem(Guid Id, Guid OrganizationId, Guid ServiceId, int ResponseMinutes, int ResolutionMinutes, bool IsArchived, DateTime CreatedAtUtc);
public sealed record SlaClockItem(Guid Id, Guid OrganizationId, Guid RequestId, Guid ServiceId, DateTime StartedAtUtc, DateTime ResponseDueAtUtc, DateTime ResolutionDueAtUtc, DateTime? FirstResponseAtUtc, DateTime? ResolvedAtUtc, SlaClockStatus Status);


