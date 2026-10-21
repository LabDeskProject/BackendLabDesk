using SLA.Domain;
using SLA.Domain.Enum;
namespace SLA.Domain.Tests;
public sealed class SlaTests
{
    private static readonly DateTime Start = new(2026, 6, 1, 10, 0, 0, DateTimeKind.Utc);
    [Fact]
    public void Policy_requires_valid_response_and_resolution_windows()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SlaPolicy.Create(Guid.NewGuid(), Guid.NewGuid(), 0, 10, Start));
        Assert.Throws<ArgumentOutOfRangeException>(() => SlaPolicy.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 5, Start));
    }
    [Fact]
    public void Clock_uses_policy_deadlines_and_records_on_time_response_and_resolution()
    {
        var org = Guid.NewGuid(); var service = Guid.NewGuid(); var policy = SlaPolicy.Create(org, service, 30, 240, Start);
        var clock = SlaClock.Start(org, Guid.NewGuid(), service, policy, Start);
        Assert.Equal(Start.AddMinutes(30), clock.ResponseDueAtUtc); Assert.Equal(Start.AddHours(4), clock.ResolutionDueAtUtc);
        clock.RecordResponse(Start.AddMinutes(20)); clock.Resolve(Start.AddHours(3));
        Assert.Equal(SlaClockStatus.Resolved, clock.Status);
    }
    [Fact]
    public void Clock_records_response_and_resolution_breaches()
    {
        var org = Guid.NewGuid(); var service = Guid.NewGuid(); var policy = SlaPolicy.Create(org, service, 10, 60, Start);
        var clock = SlaClock.Start(org, Guid.NewGuid(), service, policy, Start);
        clock.RecordResponse(Start.AddMinutes(11)); Assert.Equal(SlaClockStatus.ResponseBreached, clock.Status);
        clock.Resolve(Start.AddMinutes(61)); Assert.Equal(SlaClockStatus.ResolutionBreached, clock.Status);
    }
    [Fact]
    public void Archived_policy_cannot_be_used_for_new_clock()
    {
        var org = Guid.NewGuid(); var service = Guid.NewGuid(); var policy = SlaPolicy.Create(org, service, 10, 60, Start); policy.Archive(Start);
        Assert.Throws<InvalidOperationException>(() => SlaClock.Start(org, Guid.NewGuid(), service, policy, Start));
    }
    [Fact]
    public void Policy_rejects_cross_organization_or_service_scope()
    {
        var org = Guid.NewGuid(); var service = Guid.NewGuid(); var policy = SlaPolicy.Create(org, service, 10, 60, Start);
        Assert.Throws<InvalidOperationException>(() => SlaClock.Start(Guid.NewGuid(), Guid.NewGuid(), service, policy, Start));
        Assert.Throws<InvalidOperationException>(() => SlaClock.Start(org, Guid.NewGuid(), Guid.NewGuid(), policy, Start));
    }
    [Fact]
    public void Policy_archive_is_terminal_for_mutation()
    {
        var policy = SlaPolicy.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 60, Start); policy.Archive(Start);
        Assert.Throws<InvalidOperationException>(() => policy.Update(20, 120, Start.AddMinutes(1)));
    }
}
