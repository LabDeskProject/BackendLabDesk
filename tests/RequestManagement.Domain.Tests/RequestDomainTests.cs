using RequestManagement.Domain;
using RequestManagement.Domain.Status;

namespace RequestManagement.Domain.Tests;

public sealed class RequestDomainTests
{
    private static readonly DateTime Now = new(2026, 4, 1, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Submit_starts_in_submitted_state_and_normalizes_json()
    {
        var request = ServiceRequest.Submit(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "REQ-000001", "  My request ", null, "{ \"x\" : 1 }", Now);
        Assert.Equal(RequestStatus.Submitted, request.Status);
        Assert.Equal("My request", request.Title);
        Assert.Equal("{\"x\":1}", request.FormDataJson);
    }

    [Theory]
    [InlineData("[]")]
    [InlineData("null")]
    [InlineData("not json")]
    public void Submit_rejects_non_object_form_data(string formData)
    {
        Assert.ThrowsAny<Exception>(() => ServiceRequest.Submit(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "REQ", "title", null, formData, Now));
    }

    [Fact]
    public void Status_transitions_allow_review_then_approval_completion()
    {
        var request = ServiceRequest.Submit(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "REQ", "title", null, "{}", Now);
        request.Transition(RequestStatus.InReview, Now.AddMinutes(1));
        request.Transition(RequestStatus.Approved, Now.AddMinutes(2));
        request.Transition(RequestStatus.Completed, Now.AddMinutes(3));
        Assert.Equal(RequestStatus.Completed, request.Status);
        Assert.Equal(Now.AddMinutes(3), request.UpdatedAtUtc);
    }

    [Fact]
    public void Status_transitions_reject_invalid_terminal_changes()
    {
        var request = ServiceRequest.Submit(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "REQ", "title", null, "{}", Now);
        request.Transition(RequestStatus.Cancelled, Now.AddMinutes(1));
        Assert.Throws<InvalidOperationException>(() => request.Transition(RequestStatus.InReview, Now.AddMinutes(2)));
    }

    [Fact]
    public void Sequence_scopes_monotonic_numbers_to_organization()
    {
        var org = Guid.NewGuid();
        var sequence = RequestNumberSequence.Start(org);
        var first = sequence.Allocate(new DateOnly(2026, 4, 1));
        var second = sequence.Allocate(new DateOnly(2026, 4, 1));
        Assert.Contains(org.ToString("N")[..8].ToUpperInvariant(), first);
        Assert.EndsWith("000001", first);
        Assert.EndsWith("000002", second);
    }
}
