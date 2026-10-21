using RequestManagement.Domain;
using RequestManagement.Domain.Status;
namespace RequestManagement.Application.Contracts;
public sealed record RequestListItem(Guid Id, Guid OrganizationId, Guid ServiceId, Guid SubmittedByUserId, string RequestNumber, string Title, string? Description, string FormDataJson, RequestStatus Status, DateTime CreatedAtUtc, DateTime UpdatedAtUtc);


