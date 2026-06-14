using System.ComponentModel.DataAnnotations.Schema;

namespace PartyPlanner.Core.Entities;

[Table("AppNotifications")]
public sealed class EntityAppNotification
{
    private EntityAppNotification()
    {
        Title = string.Empty;
        Message = string.Empty;
        Type = string.Empty;
    }

    public EntityAppNotification(Guid id, Guid userId, string title, string message, string type)
    {
        Id = id;
        UserId = userId;
        Title = title;
        Message = message;
        Type = type;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public string Type { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public EntityUser? EntityUser { get; private set; }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
