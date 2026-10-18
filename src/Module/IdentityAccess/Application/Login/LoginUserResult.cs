namespace Application.Login;

public sealed record LoginUserResult(Guid UserId, string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc);
