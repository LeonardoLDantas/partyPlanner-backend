namespace PartyPlanner.Core.Entities;

public sealed class PartyTask
{
    private PartyTask()
    {
    }

    public PartyTask(Guid id, string title, string assignee, string dueDate, string description, string status, bool done)
    {
        Id = id;
        Title = title;
        Assignee = assignee;
        DueDate = dueDate;
        Description = description;
        Status = status;
        Done = done;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Assignee { get; private set; } = string.Empty;
    public string DueDate { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
    public bool Done { get; private set; }

    public void Toggle()
    {
        Done = !Done;
        Status = Done ? "Concluída" : "Pendente";
    }

    public void UpdateStatus(string status)
    {
        Status = status;
        Done = status.Equals("Concluída", StringComparison.OrdinalIgnoreCase)
            || status.Equals("Concluida", StringComparison.OrdinalIgnoreCase);
    }

    public void UpdateDetails(string title, string assignee, string description, string status)
    {
        Title = title;
        Assignee = assignee;
        Description = description;
        UpdateStatus(status);
    }
}
