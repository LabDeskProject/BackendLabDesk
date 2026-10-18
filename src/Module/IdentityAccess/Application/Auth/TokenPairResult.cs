namespace Application.Auth;

public sealed record TokenPairResult(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc)
{

}
