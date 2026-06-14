using System.ComponentModel.DataAnnotations.Schema;

namespace PartyPlanner.Core.Entities;

[Table("UserExternalLogins")]
public sealed class EntityUserExternalLogin
{
    private EntityUserExternalLogin()
    {
        Provider = string.Empty;
        ProviderUserId = string.Empty;
        Email = string.Empty;
    }

    public EntityUserExternalLogin(Guid id, Guid userId, string provider, string providerUserId, string email)
    {
        Id = id;
        UserId = userId;
        Provider = provider;
        ProviderUserId = providerUserId;
        Email = email;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Provider { get; private set; }
    public string ProviderUserId { get; private set; }
    public string Email { get; private set; }
    public DateTime LinkedAtUtc { get; private set; } = DateTime.UtcNow;
    public EntityUser EntityUser { get; private set; } = null!;
}
