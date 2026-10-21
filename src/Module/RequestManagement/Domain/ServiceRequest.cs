using RequestManagement.Domain.Status;
using System.Text.Json;

namespace RequestManagement.Domain;

public sealed class ServiceRequest
{
    private ServiceRequest() { }
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid ServiceId { get; private set; }
    public Guid SubmittedByUserId { get; private set; }
    public string RequestNumber { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string FormDataJson { get; private set; } = "{}";
    public RequestStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    public static ServiceRequest Submit(Guid organizationId, Guid serviceId, Guid userId, string requestNumber, string title, string? description, string formDataJson, DateTime nowUtc)
    {
        if (organizationId == Guid.Empty || serviceId == Guid.Empty || userId == Guid.Empty) 
            throw new ArgumentException("Organization, service, and submitter IDs are required.");
        if (string.IsNullOrWhiteSpace(requestNumber)) 
            throw new ArgumentException("Request number is required.", nameof(requestNumber));
        if (string.IsNullOrWhiteSpace(title)) 
            throw new ArgumentException("Request title is required.", nameof(title));
        using var parsed = JsonDocument.Parse(string.IsNullOrWhiteSpace(formDataJson) ? "{}" : formDataJson);
        if (parsed.RootElement.ValueKind != JsonValueKind.Object) 
            throw new ArgumentException("Form data must be a JSON object.", nameof(formDataJson));
        return new ServiceRequest { Id = Guid.NewGuid(), OrganizationId = organizationId, ServiceId = serviceId, SubmittedByUserId = userId, RequestNumber = requestNumber.Trim(), Title = title.Trim(), Description = description?.Trim(), FormDataJson = JsonSerializer.Serialize(parsed.RootElement), Status = RequestStatus.Submitted, CreatedAtUtc = nowUtc, UpdatedAtUtc = nowUtc };
    }

    public void Transition(RequestStatus next, DateTime nowUtc)
    {
        var allowed = Status switch
        {
            RequestStatus.Submitted => next is RequestStatus.InReview or RequestStatus.Cancelled,
            RequestStatus.InReview => next is RequestStatus.Approved or RequestStatus.Rejected or RequestStatus.Cancelled,
            RequestStatus.Approved => next is RequestStatus.Completed or RequestStatus.Cancelled,
            _ => false
        };
        if (!allowed) throw new InvalidOperationException($"Cannot transition request from {Status} to {next}.");
        Status = next;
        UpdatedAtUtc = nowUtc;
    }
}
