namespace ServiceCatalog.Domain;

public sealed class ServiceDefinition
{
    private ServiceDefinition() { }
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsArchived { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public static ServiceDefinition Create(Guid organizationId, string name, string? description, DateTime nowUtc)
    {
        if (organizationId == Guid.Empty) 
            throw new ArgumentException("Organization is required.", nameof(organizationId));
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Service name is required.", nameof(name));
        return new ServiceDefinition { Id = Guid.NewGuid(), OrganizationId = organizationId, Name = name.Trim(), Description = description?.Trim(), CreatedAtUtc = nowUtc, UpdatedAtUtc = nowUtc };
    }
    public void Rename(string name, string? description, DateTime nowUtc)
    {
        if (IsArchived) 
            throw new InvalidOperationException("Archived service cannot be changed.");
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Service name is required.", nameof(name));
        Name = name.Trim(); Description = description?.Trim(); UpdatedAtUtc = nowUtc;
    }
    public void Archive(DateTime nowUtc) { IsArchived = true; UpdatedAtUtc = nowUtc; }
}
