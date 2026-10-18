using Application.Auth;
using Application.Interface;
using Application.Interface.Security;
using BuildingBlocks.Application;
using BuildingBlocks.Application.CQRS;

namespace Application.RefreshTokens.RefreshToken;

public sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, TokenPairResult>
{
    private readonly IRefreshTokenRepository _tokens;
    private readonly ILoginRepository _users;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenService _tokenService;
    private readonly IUnitOfWorkIdentityAccess _unitOfWorkIdentityAccess;
    private readonly IRefreshTokenLifetime _refreshTokenLifetime;
    private readonly IAccessTokenLifetime _accessTokenLifetime;

    public RefreshTokenCommandHandler(IRefreshTokenRepository tokens, ILoginRepository users, ITokenProvider tokenProvider,
        IRefreshTokenService tokenService, IUnitOfWorkIdentityAccess unitOfWorkIdentityAccess, IRefreshTokenLifetime refreshTokenLifetime,
        IAccessTokenLifetime accessTokenLifetime)
    {
        _tokens = tokens;
        _users = users;
        _tokenProvider = tokenProvider;
        _tokenService = tokenService;
        _unitOfWorkIdentityAccess = unitOfWorkIdentityAccess;
        _refreshTokenLifetime = refreshTokenLifetime;
        _accessTokenLifetime = accessTokenLifetime;
    }

    public async Task<TokenPairResult> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var current = await _tokens.GetByHashAsync(_tokenService.HashToken(command.Token), cancellationToken);
        if (current is null) throw new UnauthorizedAccessException("Invalid refresh token.");
        if (current.RevokedAtUtc is not null)
        {
            await _tokens.RevokeFamilyAsync(current.FamilyId, now, cancellationToken);
            throw new UnauthorizedAccessException("Refresh token reuse detected; the session was revoked.");
        }
        if (!current.IsActive(now) || current.FamilyExpiresAtUtc <= now)
        {
            await _tokens.RevokeFamilyAsync(current.FamilyId, now, cancellationToken);
            throw new UnauthorizedAccessException("Refresh token has expired.");
        }

        var user = await _users.GetByIdAsync(current.UserId, cancellationToken);
        if (user is null || !user.IsActive)
        {
            await _tokens.RevokeFamilyAsync(current.FamilyId, now, cancellationToken);
            await _unitOfWorkIdentityAccess.SaveChangesAsync(cancellationToken);
            throw new UnauthorizedAccessException("User is inactive.");
        }

        var rawNextToken = _tokenService.GenerateToken();
        var nextExpiry = new[] { now.AddDays(_refreshTokenLifetime.Days), current.FamilyExpiresAtUtc }.Min();
        var next = Domain.RefreshTokens.RefreshToken.Create(user.Id, current.FamilyId, _tokenService.HashToken(rawNextToken), now,
            nextExpiry, current.FamilyExpiresAtUtc);
        if (!await _tokens.TryRotateAsync(current.Id, next, now, cancellationToken))
        {
            await _tokens.RevokeFamilyAsync(current.FamilyId, now, cancellationToken);
            throw new UnauthorizedAccessException("Refresh token reuse detected; the session was revoked.");
        }

        return new TokenPairResult(_tokenProvider.GenerateAccessToken(user), rawNextToken,
            now.AddMinutes(_accessTokenLifetime.Minutes));
    }
}
