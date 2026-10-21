using Audit.Domain;
namespace Audit.Domain.Tests;
public sealed class AuditEntryTests
{
    [Fact]
    public void Create_accepts_json_object_details_and_normalizes_fields()
    {
        var e = AuditEntry.Create(Guid.NewGuid(), Guid.NewGuid(), " RequestSubmitted ", " Request ", "abc", "trace-1", "{\"source\":\"api\"}", DateTime.UtcNow);
        Assert.Equal("RequestSubmitted", e.Action); Assert.Equal("Request", e.EntityType); Assert.Equal("abc", e.EntityId);
    }
    [Theory]
    [InlineData("[]")]
    [InlineData("null")]
    [InlineData("not-json")]
    public void Create_rejects_non_object_details(string json) => Assert.ThrowsAny<Exception>(() => AuditEntry.Create(Guid.NewGuid(), null, "Action", "Request", "id", null, json, DateTime.UtcNow));
    [Fact]
    public void Create_rejects_invalid_identity_and_missing_names()
    {
        Assert.Throws<ArgumentException>(() => AuditEntry.Create(Guid.Empty, null, "Action", "Request", "1", null, null, DateTime.UtcNow));
        Assert.Throws<ArgumentException>(() => AuditEntry.Create(Guid.NewGuid(), Guid.Empty, "Action", "Request", "1", null, null, DateTime.UtcNow));
    }
}
