using Assignment.Domain;
using Assignment.Domain.Enum;
namespace Assignment.Domain.Tests;
public sealed class AssignmentTests
{
    [Fact]
    public void Create_starts_active_with_assignee_and_timestamps()
    {
        var now = DateTime.UtcNow; var a = AssignmentRecord.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), null, " note ", now);
        Assert.Equal(AssignmentStatus.Active, a.Status); Assert.Equal("note", a.Note); Assert.Equal(now, a.AssignedAtUtc); Assert.Null(a.EndedAtUtc);
    }
    [Theory]
    [InlineData(AssignmentStatus.Reassigned)] [InlineData(AssignmentStatus.Completed)] [InlineData(AssignmentStatus.Cancelled)]
    public void Active_assignment_can_be_closed(AssignmentStatus status)
    {
        var now = DateTime.UtcNow; var a = AssignmentRecord.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), null, null, now);
        a.Close(status, now.AddMinutes(1)); Assert.Equal(status, a.Status); Assert.Equal(now.AddMinutes(1), a.EndedAtUtc);
    }
    [Fact]
    public void Closed_assignment_cannot_be_closed_twice()
    {
        var a = AssignmentRecord.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), null, null, DateTime.UtcNow);
        a.Close(AssignmentStatus.Completed, DateTime.UtcNow); Assert.Throws<InvalidOperationException>(() => a.Close(AssignmentStatus.Cancelled, DateTime.UtcNow));
    }
}
