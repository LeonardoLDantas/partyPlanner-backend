namespace PartyPlanner.Core.Entities;

public sealed class AppNotification
{
    private AppNotification()
    {
        Title = string.Empty;
        Message = string.Empty;
        Type = string.Empty;
    }

    public AppNotification(Guid id, Guid userId, string title, string message, string type)
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
    public User? User { get; private set; }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
