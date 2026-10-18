using Application.Interface;
using Application.Interface.Security;
using BuildingBlocks.Application;
using BuildingBlocks.Application.CQRS;

namespace Application.RefreshTokens.RevokeRefreshToken;

public sealed class RevokeRefreshTokenCommandHandler : ICommandHandler<RevokeRefreshTokenCommand, bool>
{
    private readonly IRefreshTokenRepository _tokens;
    private readonly IRefreshTokenService _tokenService;
    private readonly IUnitOfWorkIdentityAccess _unitOfWorkIdentityAccess;

    public RevokeRefreshTokenCommandHandler(IRefreshTokenRepository tokens, IRefreshTokenService tokenService, IUnitOfWorkIdentityAccess unitOfWorkIdentityAccess)
    {
        _tokens = tokens;
        _tokenService = tokenService;
        _unitOfWorkIdentityAccess = unitOfWorkIdentityAccess;
    }

    public async Task<bool> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var token = await _tokens.GetByHashAsync(_tokenService.HashToken(command.Token), cancellationToken);
        if (token is null) return true;
        await _tokens.RevokeFamilyAsync(token.FamilyId, DateTime.UtcNow, cancellationToken);
        await _unitOfWorkIdentityAccess.SaveChangesAsync(cancellationToken);
        return true;
    }
}
