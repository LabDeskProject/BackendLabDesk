using Application.Interface;
using Application.Interface.Security;
using Application.Auth;
using BuildingBlocks.Application;
using BuildingBlocks.Application.CQRS;

namespace Application.Login;

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResult>
{
    private readonly ILoginRepository _loginRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenService _refreshTokens;
    private readonly IRefreshTokenRepository _refreshRepository;
    private readonly IUnitOfWorkIdentityAccess _unitOfWorkIdentityAccess;
    private readonly Verify _verify;
    private readonly IRefreshTokenLifetime _refreshTokenLifetime;
    private readonly IAccessTokenLifetime _accessTokenLifetime;

    public LoginUserCommandHandler(ILoginRepository loginRepository, ITokenProvider tokenProvider,
        Verify verify, IRefreshTokenService refreshTokens, IRefreshTokenRepository refreshRepository, IUnitOfWorkIdentityAccess unitOfWorkIdentityAccess,
        IRefreshTokenLifetime refreshTokenLifetime, IAccessTokenLifetime accessTokenLifetime)
    {
        _loginRepository = loginRepository;
        _tokenProvider = tokenProvider;
        _verify = verify;
        _refreshTokens = refreshTokens;
        _refreshRepository = refreshRepository;
        _unitOfWorkIdentityAccess = unitOfWorkIdentityAccess;
        _refreshTokenLifetime = refreshTokenLifetime;
        _accessTokenLifetime = accessTokenLifetime;
    }

    public async Task<LoginUserResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _loginRepository.GetByEmailAsync(command.Email.Trim().ToLowerInvariant(), cancellationToken);
        if (user is null || !user.IsActive || !_verify.Verify(command.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var now = DateTime.UtcNow;
        var refreshToken = _refreshTokens.GenerateToken();
        var familyExpiresAt = now.AddDays(_refreshTokenLifetime.AbsoluteDays);
        var refreshRecord = Domain.RefreshTokens.RefreshToken.Create(user.Id, Guid.NewGuid(), _refreshTokens.HashToken(refreshToken),
            now, now.AddDays(_refreshTokenLifetime.Days), familyExpiresAt);
        _refreshRepository.Add(refreshRecord);
        await _unitOfWorkIdentityAccess.SaveChangesAsync(cancellationToken);

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        return new LoginUserResult(user.Id, accessToken, refreshToken, now.AddMinutes(_accessTokenLifetime.Minutes));
    }
}
