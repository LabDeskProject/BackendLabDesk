namespace OrganizationManagement.Domain;

public sealed class TeamMember
{
    private TeamMember() { }
    public Guid TeamId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public Team Team { get; private set; } = null!;
    public static TeamMember Create(Guid teamId, Guid userId)
    {
        if (teamId == Guid.Empty || userId == Guid.Empty) 
            throw new ArgumentException("Team and user are required.");
        return new TeamMember { TeamId = teamId, UserId = userId, JoinedAt = DateTime.UtcNow };
    }
}
