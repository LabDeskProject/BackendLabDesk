namespace OrganizationManagement.Domain;

public sealed class Organization
{
    private Organization() { }

    private Organization(Guid id, string name, string slug, string? description, DateTime createdAtUtc)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Description = description;
        CreatedAt = createdAtUtc;
        UpdatedAt = createdAtUtc;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsArchived { get; private set; }
    public ICollection<Team> Teams { get; private set; } = new List<Team>();
    public ICollection<OrganizationUser> Members { get; private set; } = new List<OrganizationUser>();

    public static Organization Create(string name, string slug, string? description, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Organization name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(slug)) 
            throw new ArgumentException("Organization slug is required.", nameof(slug));
        var normalizedSlug = slug.Trim().ToLowerInvariant();
        if (!System.Text.RegularExpressions.Regex.IsMatch(normalizedSlug, "^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            throw new ArgumentException("Slug must contain lowercase letters, numbers, and single hyphen separators.", nameof(slug));
        return new Organization(Guid.NewGuid(), name.Trim(), normalizedSlug, description?.Trim(), nowUtc);
    }

    public void Update(string name, string slug, string? description, DateTime updatedAtUtc)
    {
        if (IsArchived) 
            throw new InvalidOperationException("Archived organizations cannot be changed.");
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Organization name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(slug)) throw new ArgumentException("Organization slug is required.", nameof(slug));
        var normalizedSlug = slug.Trim().ToLowerInvariant();
        if (!System.Text.RegularExpressions.Regex.IsMatch(normalizedSlug, "^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            throw new ArgumentException("Slug must contain lowercase letters, numbers, and single hyphen separators.", nameof(slug));
        Name = name.Trim();
        Slug = normalizedSlug;
        Description = description?.Trim();
        UpdatedAt = updatedAtUtc;
    }

    public void Archive(DateTime archivedAtUtc)
    {
        if (IsArchived) return;
        IsArchived = true;
        UpdatedAt = archivedAtUtc;
    }
}
