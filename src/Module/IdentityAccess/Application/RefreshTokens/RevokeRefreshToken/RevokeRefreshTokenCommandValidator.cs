using FluentValidation;

namespace Application.RefreshTokens.RevokeRefreshToken;

public sealed class RevokeRefreshTokenCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeRefreshTokenCommandValidator() => RuleFor(x => x.Token).NotEmpty().MaximumLength(128);
}
