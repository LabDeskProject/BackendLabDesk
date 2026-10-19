using Application.Interface.Security;
using Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication;

public sealed class JwtTokenProvider : ITokenProvider
{
    private readonly JwtSettings _jwtSettings;
    private readonly IAccessTokenLifetime _accessTokenLifetime;

    public JwtTokenProvider(IOptions<JwtSettings> jwtOptions, IAccessTokenLifetime accessTokenLifetime)
    {
        _jwtSettings = jwtOptions.Value;
        _accessTokenLifetime = accessTokenLifetime;
    }

    public string GenerateAccessToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        var now = DateTime.UtcNow;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new("principal_type", "user"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.IsGlobalAdmin ? "Admin" : "User")
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, now,
            now.AddMinutes(_accessTokenLifetime.Minutes), credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
