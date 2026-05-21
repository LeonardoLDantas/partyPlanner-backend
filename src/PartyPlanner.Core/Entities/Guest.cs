using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.Entities;

public sealed class Guest
{
    private Guest()
    {
    }

    public Guest(Guid id, string name, string group, GuestType type, string status, string invitationToken, string email, string phoneNumber)
    {
        Id = id;
        Name = name;
        Group = group;
        Type = type;
        Status = status;
        InvitationToken = invitationToken;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Group { get; private set; } = string.Empty;
    public GuestType Type { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public string InvitationToken { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;

    public void UpdateStatus(string status)
    {
        Status = status;
    }
}
