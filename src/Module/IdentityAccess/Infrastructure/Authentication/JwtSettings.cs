using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Authentication;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";
    [Required] public string Issuer { get; set; } = string.Empty;
    [Required] public string Audience { get; set; } = string.Empty;
    [Required] public string SecretKey { get; set; } = string.Empty;
    [Range(1, 60)] public int ExpirationInMinutes { get; set; } = 10;
}
