using Application.Interface;
using Domain.RefreshTokens;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IdentityAccessDbContext _dbContext;

    public RefreshTokenRepository(IdentityAccessDbContext dbContext) => _dbContext = dbContext;

    public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken) =>
        _dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

    public void Add(RefreshToken token) => _dbContext.RefreshTokens.Add(token);

    public async Task<bool> TryRotateAsync(Guid currentTokenId, RefreshToken nextToken, DateTime revokedAtUtc, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        var affected = await _dbContext.RefreshTokens
            .Where(x => x.Id == currentTokenId && x.RevokedAtUtc == null && x.ExpiresAtUtc > revokedAtUtc && x.FamilyExpiresAtUtc > revokedAtUtc)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.RevokedAtUtc, revokedAtUtc)
                .SetProperty(x => x.ReplacedByTokenId, (Guid?)nextToken.Id), cancellationToken);
        if (affected == 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }
        _dbContext.RefreshTokens.Add(nextToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    public async Task RevokeFamilyAsync(Guid familyId, DateTime revokedAtUtc, CancellationToken cancellationToken)
    {
        await _dbContext.RefreshTokens
            .Where(x => x.FamilyId == familyId && x.RevokedAtUtc == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.RevokedAtUtc, revokedAtUtc), cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
