namespace PartyPlanner.Core.Entities;

public sealed class PartyTask
{
    private PartyTask()
    {
    }

    public PartyTask(Guid id, string title, string assignee, bool done)
    {
        Id = id;
        Title = title;
        Assignee = assignee;
        Done = done;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Assignee { get; private set; } = string.Empty;
    public bool Done { get; private set; }

    public void Toggle()
    {
        Done = !Done;
    }
}
