using Assignment.Domain.Enum;

namespace Assignment.Domain;
public sealed class AssignmentRecord
{
    private AssignmentRecord() { }
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid RequestId { get; private set; }
    public Guid AssignedToUserId { get; private set; }
    public Guid AssignedByUserId { get; private set; }
    public Guid? TeamId { get; private set; }
    public string? Note { get; private set; }
    public AssignmentStatus Status { get; private set; }
    public DateTime AssignedAtUtc { get; private set; }
    public DateTime? EndedAtUtc { get; private set; }

    public static AssignmentRecord Create(Guid organizationId, Guid requestId, Guid assigneeUserId, Guid assignedByUserId, Guid? teamId, string? note, DateTime nowUtc)
    {
        if (organizationId == Guid.Empty || requestId == Guid.Empty || assigneeUserId == Guid.Empty || assignedByUserId == Guid.Empty) throw new ArgumentException("Organization, request, assignee, and assigning user are required.");
        if (teamId == Guid.Empty) throw new ArgumentException("Team ID cannot be empty.", nameof(teamId));
        return new AssignmentRecord { Id = Guid.NewGuid(), OrganizationId = organizationId, RequestId = requestId, AssignedToUserId = assigneeUserId, AssignedByUserId = assignedByUserId, TeamId = teamId, Note = note?.Trim(), Status = AssignmentStatus.Active, AssignedAtUtc = nowUtc };
    }

    public void Close(AssignmentStatus terminalStatus, DateTime nowUtc)
    {
        if (Status != AssignmentStatus.Active) throw new InvalidOperationException("Only active assignments can be closed.");
        if (terminalStatus is not (AssignmentStatus.Reassigned or AssignmentStatus.Completed or AssignmentStatus.Cancelled)) throw new ArgumentException("Invalid assignment closing status.", nameof(terminalStatus));
        Status = terminalStatus;
        EndedAtUtc = nowUtc;
    }
}
