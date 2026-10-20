namespace OrganizationManagement.Domain;

public sealed class OrganizationUser
{
    private OrganizationUser() { }
    public Guid OrganizationId { get; private set; }
    public Guid UserId { get; private set; }
    public OrganizationRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public DateTime? RemovedAtUtc { get; private set; }
    public bool IsActive => RemovedAtUtc is null;
    public OrganizationUser(Guid organizationId, Guid userId, OrganizationRole role, DateTime joinedAt)
    {
        if (organizationId == Guid.Empty || userId == Guid.Empty) 
            throw new ArgumentException("Organization and user are required.");
        if (!Enum.IsDefined(role)) 
            throw new ArgumentOutOfRangeException(nameof(role));
        OrganizationId = organizationId; UserId = userId; Role = role; JoinedAt = joinedAt;
    }
    public void ChangeRole(OrganizationRole role)
    {
        if (!IsActive) 
            throw new InvalidOperationException("Removed memberships cannot be changed.");
        if (!Enum.IsDefined(role)) 
            throw new ArgumentOutOfRangeException(nameof(role));
        Role = role;
    }
    public void Remove(DateTime atUtc) { RemovedAtUtc ??= atUtc; }
    public void Reactivate(OrganizationRole role, DateTime atUtc)
    {
        if (IsActive) 
            throw new InvalidOperationException("Membership is already active.");
        if (!Enum.IsDefined(role)) 
            throw new ArgumentOutOfRangeException(nameof(role));
        Role = role; RemovedAtUtc = null; JoinedAt = atUtc;
    }
}
