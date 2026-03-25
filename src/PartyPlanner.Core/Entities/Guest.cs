namespace PartyPlanner.Core.Entities;

public sealed class Guest
{
    private Guest()
    {
    }

    public Guest(Guid id, string name, string group, string status)
    {
        Id = id;
        Name = name;
        Group = group;
        Status = status;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Group { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
}
