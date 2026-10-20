namespace OrganizationManagement.Domain;

public sealed class Team
{
    private Team() { }
    private Team(Guid id, Guid organizationId, string name, string? description, DateTime nowUtc)
    { 
        Id = id; 
        OrganizationId = organizationId; 
        Name = name; 
        Description = description; 
        CreatedAt = nowUtc; 
        UpdatedAt = nowUtc; 
    }
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsArchived { get; private set; }
    public Guid? TeamLeadUserId { get; private set; }
    public Organization Organization { get; private set; } = null!;
    public ICollection<TeamMember> Members { get; private set; } = new List<TeamMember>();
    public static Team Create(Guid organizationId, string name, string? description, DateTime nowUtc, Guid? teamLeadUserId = null)
    {
        if (organizationId == Guid.Empty) 
            throw new ArgumentException("Organization is required.", nameof(organizationId));
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Team name is required.", nameof(name));
        return new Team(Guid.NewGuid(), organizationId, name.Trim(), description?.Trim(), nowUtc) { TeamLeadUserId = teamLeadUserId };
    }
    public void Update(string name, string? description, DateTime updatedAtUtc, Guid? teamLeadUserId = null)
    {
        if (IsArchived) 
            throw new InvalidOperationException("Archived teams cannot be changed.");
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Team name is required.", nameof(name));
        Name = name.Trim(); Description = description?.Trim(); TeamLeadUserId = teamLeadUserId; UpdatedAt = updatedAtUtc;
    }
    public void Archive(DateTime archivedAtUtc) 
    { 
        if (IsArchived) return; 
        IsArchived = true; 
        UpdatedAt = archivedAtUtc; 
    }
}
