using Application.Interface.Security;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication.AccessToken;

public sealed class AccessTokenLifetime : IAccessTokenLifetime
{
    public AccessTokenLifetime(IOptions<JwtSettings> settings) => Minutes = settings.Value.ExpirationInMinutes;
    public int Minutes { get; }
}
