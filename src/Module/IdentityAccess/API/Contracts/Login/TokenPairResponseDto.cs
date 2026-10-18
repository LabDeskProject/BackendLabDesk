namespace IdentityAccess.API.Contracts.Login;

public sealed record TokenPairResponseDto(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc);
