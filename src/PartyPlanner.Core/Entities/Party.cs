using System.Globalization;
using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.Entities;

public sealed class Party
{
    private Party()
    {
        Name = string.Empty;
        Category = PartyCategory.Outros;
        Date = string.Empty;
        Location = string.Empty;
        Budget = null!;
    }

    public Party(Guid id, Guid ownerUserId, string name, PartyCategory category, string date, string location, Budget budget)
    {
        Id = id;
        OwnerUserId = ownerUserId;
        Name = name;
        Category = category;
        Date = date;
        Location = location;
        Budget = budget;
    }

    public Guid Id { get; private set; }
    public Guid OwnerUserId { get; private set; }
    public string Name { get; private set; }
    public PartyCategory Category { get; private set; }
    public string Date { get; private set; }
    public string Location { get; private set; }
    public List<PartyTask> Tasks { get; private set; } = [];
    public List<Guest> Guests { get; private set; } = [];
    public Budget Budget { get; private set; }
    public User Owner { get; private set; } = null!;

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

    public void UpdateDetails(string name, PartyCategory category, string date, string location, decimal estimatedBudget)
    {
        Name = name;
        Category = category;
        Date = date;
        Location = location;
        Budget.UpdateEstimated(estimatedBudget);
    }

    public bool CanBeEditedOn(DateOnly referenceDate)
    {
        if (!TryGetEventDate(out var eventDate))
        {
            return true;
        }

        return referenceDate <= eventDate;
    }

    public void EnsureEditableOn(DateOnly referenceDate)
    {
        if (!CanBeEditedOn(referenceDate))
        {
            throw new InvalidOperationException("A festa nao pode mais ser editada apos a data de realizacao.");
        }
    }

    private bool TryGetEventDate(out DateOnly eventDate)
    {
        return DateOnly.TryParseExact(
                   Date,
                   "yyyy-MM-dd",
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out eventDate)
               || DateOnly.TryParse(
                   Date,
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out eventDate);
    }
}
