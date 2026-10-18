using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Authentication.RefreshToken;

public sealed class RefreshTokenSettings
{
    public const string SectionName = "RefreshToken";
    [Range(1, 30)] public int ExpirationInDays { get; set; } = 14;
    [Range(1, 90)] public int AbsoluteExpirationInDays { get; set; } = 30;
}
