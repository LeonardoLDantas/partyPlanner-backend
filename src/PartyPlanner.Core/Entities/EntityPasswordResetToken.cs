using System.ComponentModel.DataAnnotations.Schema;

namespace PartyPlanner.Core.Entities;

[Table("PasswordResetTokens")]
public sealed class EntityPasswordResetToken
{
    private EntityPasswordResetToken() { Token = string.Empty; }

    public EntityPasswordResetToken(Guid id, Guid userId, string token, DateTime expiresAtUtc)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiresAtUtc = expiresAtUtc;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? UsedAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public bool IsValid => UsedAtUtc is null && DateTime.UtcNow < ExpiresAtUtc;

    public void MarkAsUsed() => UsedAtUtc = DateTime.UtcNow;
}
