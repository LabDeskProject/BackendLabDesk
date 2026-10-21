namespace SLA.Domain;

public sealed class SlaPolicy
{
    private SlaPolicy() { }
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid ServiceId { get; private set; }
    public int ResponseMinutes { get; private set; }
    public int ResolutionMinutes { get; private set; }
    public bool IsArchived { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public byte[] RowVersion { get; private set; } = [];
    public static SlaPolicy Create(Guid organizationId, Guid serviceId, int responseMinutes, int resolutionMinutes, DateTime nowUtc)
    {
        if (organizationId == Guid.Empty || serviceId == Guid.Empty) 
            throw new ArgumentException("Organization and service are required.");
        if (responseMinutes <= 0 || resolutionMinutes < responseMinutes) 
            throw new ArgumentOutOfRangeException(nameof(responseMinutes), "Resolution time must be greater than or equal to positive response time.");
        return new SlaPolicy { Id = Guid.NewGuid(), OrganizationId = organizationId, ServiceId = serviceId, ResponseMinutes = responseMinutes, ResolutionMinutes = resolutionMinutes, CreatedAtUtc = nowUtc, UpdatedAtUtc = nowUtc };
    }
    public void Update(int responseMinutes, int resolutionMinutes, DateTime nowUtc)
    {
        if (IsArchived) 
            throw new InvalidOperationException("Archived SLA policy cannot be changed.");
        if (responseMinutes <= 0 || resolutionMinutes < responseMinutes) 
            throw new ArgumentOutOfRangeException(nameof(responseMinutes), "Resolution time must be greater than or equal to positive response time.");
        ResponseMinutes = responseMinutes; ResolutionMinutes = resolutionMinutes; UpdatedAtUtc = nowUtc;
    }
    public void Archive(DateTime nowUtc) { IsArchived = true; UpdatedAtUtc = nowUtc; }
}
