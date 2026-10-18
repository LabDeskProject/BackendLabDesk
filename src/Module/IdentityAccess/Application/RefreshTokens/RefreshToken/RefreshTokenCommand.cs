using Application.Auth;
using BuildingBlocks.Application.CQRS;

namespace Application.RefreshTokens.RefreshToken;

public sealed record RefreshTokenCommand(string Token) : ICommand<TokenPairResult>;
