using OrganizationManagement.Domain;

namespace OrganizationManagement.Domain.Tests;

public sealed class OrganizationDomainTests
{
    private static readonly DateTime Now = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Create_normalizes_slug_and_trims_names()
    {
        var org = Organization.Create("  Lab Desk ", "LAB-DESK", "  desc  ", Now);
        Assert.Equal("Lab Desk", org.Name); Assert.Equal("lab-desk", org.Slug); Assert.Equal("desc", org.Description); Assert.Equal(Now, org.UpdatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Upper Case")]
    [InlineData("bad--slug")]
    [InlineData("-leading")]
    public void Create_rejects_invalid_slug(string slug) => Assert.Throws<ArgumentException>(() => Organization.Create("Org", slug, null, Now));

    [Fact]
    public void Archive_is_idempotent_and_updates_timestamp()
    {
        var org = Organization.Create("Org", "org", null, Now); org.Archive(Now.AddHours(2)); org.Archive(Now.AddHours(5));
        Assert.True(org.IsArchived); Assert.Equal(Now.AddHours(2), org.UpdatedAt);
    }

    [Fact]
    public void Archived_organization_cannot_be_updated()
    {
        var org = Organization.Create("Org", "org", null, Now); org.Archive(Now.AddHours(1));
        Assert.Throws<InvalidOperationException>(() => org.Update("New", "new", null, Now.AddHours(2)));
    }

    [Fact]
    public void Team_requires_organization_and_nonblank_name()
    {
        Assert.Throws<ArgumentException>(() => Team.Create(Guid.Empty, "Team", null, Now)); Assert.Throws<ArgumentException>(() => Team.Create(Guid.NewGuid(), "  ", null, Now));
    }

    [Fact]
    public void Team_archive_prevents_rename()
    {
        var team = Team.Create(Guid.NewGuid(), "Team", null, Now); team.Archive(Now.AddMinutes(1));
        Assert.Throws<InvalidOperationException>(() => team.Update("New", null, Now.AddMinutes(2)));
    }

    [Fact]
    public void Service_requires_team_and_updates_activation_timestamp()
    {
        Assert.Throws<ArgumentException>(() => Service.Create(Guid.NewGuid(), Guid.Empty, "Service", null, Now));
        var service = Service.Create(Guid.NewGuid(), Guid.NewGuid(), " Service ", null, Now); service.Deactivate(Now.AddMinutes(1));
        Assert.False(service.IsActive); Assert.Equal(Now.AddMinutes(1), service.UpdatedAt);
    }
}
