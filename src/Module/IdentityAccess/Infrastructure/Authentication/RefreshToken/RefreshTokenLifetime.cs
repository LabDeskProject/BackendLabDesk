using Application.Interface.Security;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication.RefreshToken;

public sealed class RefreshTokenLifetime : IRefreshTokenLifetime
{
    public RefreshTokenLifetime(IOptions<RefreshTokenSettings> settings)
    {
        Days = settings.Value.ExpirationInDays;
        AbsoluteDays = settings.Value.AbsoluteExpirationInDays;
    }
    public int Days { get; }
    public int AbsoluteDays { get; }
}
