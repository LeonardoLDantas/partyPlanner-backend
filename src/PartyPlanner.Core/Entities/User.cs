namespace PartyPlanner.Core.Entities;

public sealed class User
{
    private User()
    {
        Name = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
    }

    public User(Guid id, string name, string email, string passwordHash, bool isEmailConfirmed)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        IsEmailConfirmed = isEmailConfirmed;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool IsEmailConfirmed { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public List<UserExternalLogin> ExternalLogins { get; private set; } = [];
    public List<AppNotification> Notifications { get; private set; } = [];
    public List<Party> Parties { get; private set; } = [];

    public void ConfirmEmail()
    {
        IsEmailConfirmed = true;
    }

    public void AddExternalLogin(UserExternalLogin login)
    {
        var existing = ExternalLogins.FirstOrDefault(current =>
            current.Provider == login.Provider &&
            current.ProviderUserId == login.ProviderUserId);

        if (existing is null)
        {
            ExternalLogins.Add(login);
        }
    }
}
