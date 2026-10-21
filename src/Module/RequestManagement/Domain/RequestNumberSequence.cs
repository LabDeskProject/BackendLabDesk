namespace RequestManagement.Domain;

public sealed class RequestNumberSequence
{
    private RequestNumberSequence() { }
    public Guid OrganizationId { get; private set; }
    public long NextValue { get; private set; }
    public byte[] RowVersion { get; private set; } = [];
    public static RequestNumberSequence Start(Guid organizationId) => organizationId == Guid.Empty ? 
        throw new ArgumentException("Organization is required.", nameof(organizationId)) : new() { OrganizationId = organizationId, NextValue = 1 };
    public string Allocate(DateOnly date)
    {
        var value = NextValue++;
        return $"REQ-{OrganizationId.ToString("N")[..8].ToUpperInvariant()}-{date:yyyyMMdd}-{value:D6}";
    }
}
