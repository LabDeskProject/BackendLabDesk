using BuildingBlocks.Domain;

namespace Domain.RefreshTokens;

public sealed class RefreshToken : Entity
{
    private RefreshToken() { }

    private RefreshToken(Guid userId, Guid familyId, string tokenHash, DateTime createdAtUtc, DateTime expiresAtUtc, DateTime familyExpiresAtUtc)
    {
        UserId = userId;
        FamilyId = familyId;
        TokenHash = tokenHash;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
        FamilyExpiresAtUtc = familyExpiresAtUtc;
    }

    public Guid UserId { get; private set; }
    public Guid FamilyId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime FamilyExpiresAtUtc { get; private set; }
    public byte[] Version { get; private set; } = Array.Empty<byte>();
    public DateTime? RevokedAtUtc { get; private set; }
    public Guid? ReplacedByTokenId { get; private set; }

    public bool IsActive(DateTime nowUtc) => RevokedAtUtc is null && ExpiresAtUtc > nowUtc;

    public static RefreshToken Create(Guid userId, Guid familyId, string tokenHash, DateTime createdAtUtc, DateTime expiresAtUtc, DateTime familyExpiresAtUtc)
    {
        if (userId == Guid.Empty) 
            throw new ArgumentException("User id is required.", nameof(userId));
        if (familyId == Guid.Empty) 
            throw new ArgumentException("Family id is required.", nameof(familyId));
        if (string.IsNullOrWhiteSpace(tokenHash)) 
            throw new ArgumentException("Token hash is required.", nameof(tokenHash));
        if (expiresAtUtc <= createdAtUtc) 
            throw new ArgumentException("Expiry must be after creation.", nameof(expiresAtUtc));
        if (familyExpiresAtUtc < expiresAtUtc) 
            throw new ArgumentException("Family expiry cannot precede token expiry.", nameof(familyExpiresAtUtc));
        return new RefreshToken(userId, familyId, tokenHash, createdAtUtc, expiresAtUtc, familyExpiresAtUtc);
    }

    public void Revoke(DateTime revokedAtUtc, Guid? replacedByTokenId = null)
    {
        if (RevokedAtUtc is not null) return;
        RevokedAtUtc = revokedAtUtc;
        ReplacedByTokenId = replacedByTokenId;
    }
}
