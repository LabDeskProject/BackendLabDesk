namespace OrganizationManagement.Domain;

public sealed class Service
{
    private Service() { }
    private Service(Guid id, Guid organizationId, Guid responsibleTeamId, string name, string? description, DateTime nowUtc)
    { 
        Id = id; 
        OrganizationId = organizationId; 
        ResponsibleTeamId = responsibleTeamId; 
        Name = name; Description = description; 
        IsActive = true; 
        CreatedAt = nowUtc; 
        UpdatedAt = nowUtc; 
    }
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid ResponsibleTeamId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Organization Organization { get; private set; } = null!;
    public Team ResponsibleTeam { get; private set; } = null!;
    public static Service Create(Guid organizationId, Guid responsibleTeamId, string name, string? description, DateTime nowUtc)
    {
        if (organizationId == Guid.Empty) 
            throw new ArgumentException("Organization is required.", nameof(organizationId));
        if (responsibleTeamId == Guid.Empty) 
            throw new ArgumentException("Responsible team is required.", nameof(responsibleTeamId));
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Service name is required.", nameof(name));
        return new Service(Guid.NewGuid(), organizationId, responsibleTeamId, name.Trim(), description?.Trim(), nowUtc);
    }
    public void Update(string name, string? description, Guid responsibleTeamId, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Service name is required.", nameof(name));
        if (responsibleTeamId == Guid.Empty) 
            throw new ArgumentException("Responsible team is required.", nameof(responsibleTeamId));
        Name = name.Trim(); Description = description?.Trim(); ResponsibleTeamId = responsibleTeamId; UpdatedAt = nowUtc;
    }
    public void Deactivate(DateTime nowUtc) { IsActive = false; UpdatedAt = nowUtc; }
    public void Activate(DateTime nowUtc) { IsActive = true; UpdatedAt = nowUtc; }
}
