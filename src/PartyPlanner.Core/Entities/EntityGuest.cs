using System.ComponentModel.DataAnnotations.Schema;
using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.Entities;

[Table("Guests")]
public sealed class EntityGuest
{
    private EntityGuest()
    {
    }

    public EntityGuest(Guid id, string name, GuestGroup group, GuestType type, string status, string invitationToken, string email, string phoneNumber)
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
    public GuestGroup Group { get; private set; }
    public GuestType Type { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public string InvitationToken { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;

    public void UpdateStatus(string status)
    {
        Status = status;
    }

    public void UpdateDetails(string name, GuestGroup group, GuestType type, string email, string phoneNumber)
    {
        Name = name;
        Group = group;
        Type = type;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}
