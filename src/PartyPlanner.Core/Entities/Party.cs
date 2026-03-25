namespace PartyPlanner.Core.Entities;

public sealed class Party
{
    private Party()
    {
        Name = string.Empty;
        Category = string.Empty;
        Date = string.Empty;
        Location = string.Empty;
        Budget = null!;
    }

    public Party(Guid id, string name, string category, string date, string location, Budget budget)
    {
        Id = id;
        Name = name;
        Category = category;
        Date = date;
        Location = location;
        Budget = budget;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Category { get; private set; }
    public string Date { get; private set; }
    public string Location { get; private set; }
    public List<PartyTask> Tasks { get; private set; } = [];
    public List<Guest> Guests { get; private set; } = [];
    public Budget Budget { get; private set; }

    public void AddTask(PartyTask task)
    {
        Tasks.Insert(0, task);
    }

    public void AddGuest(Guest guest)
    {
        Guests.Insert(0, guest);
    }

    public bool ToggleTask(Guid taskId)
    {
        var task = Tasks.FirstOrDefault(currentTask => currentTask.Id == taskId);
        if (task is null)
        {
            return false;
        }

        task.Toggle();
        return true;
    }

    public void AddBudgetItem(BudgetItem item)
    {
        Budget.AddItem(item);
    }
}
