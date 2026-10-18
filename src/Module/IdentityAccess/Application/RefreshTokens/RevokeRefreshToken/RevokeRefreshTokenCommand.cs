using BuildingBlocks.Application.CQRS;

namespace Application.RefreshTokens.RevokeRefreshToken;

public sealed record RevokeRefreshTokenCommand(string Token) : ICommand<bool>;
