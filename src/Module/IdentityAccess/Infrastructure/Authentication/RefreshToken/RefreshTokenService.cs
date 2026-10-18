using Application.Interface.Security;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication.RefreshToken;

public sealed class RefreshTokenService : IRefreshTokenService
{
    public string GenerateToken() => Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(64));

    public string HashToken(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        return Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(token)));
    }
}
