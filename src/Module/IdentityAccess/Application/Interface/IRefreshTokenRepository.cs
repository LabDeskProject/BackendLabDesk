using Domain.RefreshTokens;

namespace Application.Interface;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken);
    void Add(RefreshToken token);
    Task<bool> TryRotateAsync(Guid currentTokenId, RefreshToken nextToken, DateTime revokedAtUtc, CancellationToken cancellationToken);
    Task RevokeFamilyAsync(Guid familyId, DateTime revokedAtUtc, CancellationToken cancellationToken);
}
