namespace PartyPlanner.Core.Entities;

public sealed class PartyTask
{
    private PartyTask()
    {
    }

    public PartyTask(Guid id, string title, string assignee, string dueDate, string status, bool done)
    {
        Id = id;
        Title = title;
        Assignee = assignee;
        DueDate = dueDate;
        Status = status;
        Done = done;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Assignee { get; private set; } = string.Empty;
    public string DueDate { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
    public bool Done { get; private set; }

    public void Toggle()
    {
        Done = !Done;
        Status = Done ? "Concluida" : "Pendente";
    }
}
