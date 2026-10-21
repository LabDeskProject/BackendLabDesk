using OrganizationManagement.Domain;
using Xunit;
namespace OrganizationManagement.Domain.Tests;
public sealed class MembershipLifecycleTests
{
    [Fact]
    public void RemoveAndReactivate_PreservesMembershipIdentityAndUpdatesRole()
    {
        var org = Guid.NewGuid(); var user = Guid.NewGuid(); var now = DateTime.UtcNow;
        var membership = new OrganizationUser(org, user, OrganizationRole.Member, now);
        membership.Remove(now.AddMinutes(1));
        Assert.False(membership.IsActive);
        membership.Reactivate(OrganizationRole.OrganizationAdmin, now.AddMinutes(2));
        Assert.True(membership.IsActive);
        Assert.Equal(OrganizationRole.OrganizationAdmin, membership.Role);
        Assert.Equal(now.AddMinutes(2), membership.JoinedAt);
    }
    [Fact]
    public void RemovedMembership_CannotChangeRoleUntilReactivated()
    {
        var membership = new OrganizationUser(Guid.NewGuid(), Guid.NewGuid(), OrganizationRole.Member, DateTime.UtcNow);
        membership.Remove(DateTime.UtcNow);
        Assert.Throws<InvalidOperationException>(() => membership.ChangeRole(OrganizationRole.OrganizationAdmin));
        membership.Reactivate(OrganizationRole.Member, DateTime.UtcNow);
        Assert.Throws<InvalidOperationException>(() => membership.Reactivate(OrganizationRole.Member, DateTime.UtcNow));
    }
}
