using Assignment.Domain;
using Assignment.Domain.Enum;

namespace Assignment.Application.Contracts;

public sealed record AssignmentListItem(Guid Id, Guid OrganizationId, Guid RequestId, Guid AssignedToUserId, Guid AssignedByUserId, Guid? TeamId, string? Note, AssignmentStatus Status, DateTime AssignedAtUtc, DateTime? EndedAtUtc);
