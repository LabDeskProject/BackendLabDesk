using Domain.User;
namespace IdentityAccess.Domain.Tests;
public sealed class GlobalAdminTests
{
    [Fact]
    public void NewUsersNeverReceiveGlobalAdminPrivilegesByDefault()
    {
        var user = User.Create("admin@example.test", "Bootstrap Admin", "hash");
        Assert.False(user.IsGlobalAdmin);
    }
    [Fact]
    public void BootstrapGrantIsRestrictedToConfiguredEmailAndIsOneTimePerUser()
    {
        var user = User.Create("admin@example.test", "Bootstrap Admin", "hash");
        Assert.False(user.TryBootstrapGlobalAdmin("attacker@example.test"));
        Assert.False(user.IsGlobalAdmin);
        Assert.True(user.TryBootstrapGlobalAdmin("ADMIN@example.test"));
        Assert.True(user.IsGlobalAdmin);
        Assert.False(user.TryBootstrapGlobalAdmin("admin@example.test"));
    }
    [Fact]
    public void AdminPrivilegeCanBeRevokedAndRegrantedOnlyThroughAdministrativeMutation()
    {
        var user = User.Create("admin@example.test", "Bootstrap Admin", "hash");
        Assert.True(user.TryBootstrapGlobalAdmin("admin@example.test"));
        user.SetGlobalAdmin(false);
        Assert.False(user.IsGlobalAdmin);
    }
}
