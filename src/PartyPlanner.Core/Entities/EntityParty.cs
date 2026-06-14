using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using PartyPlanner.Core.Enums;
using PartyPlanner.Core.Exceptions;

namespace PartyPlanner.Core.Entities;

[Table("Parties")]
public sealed class EntityParty
{
    private EntityParty()
    {
        Name = string.Empty;
        Category = PartyCategory.Outros;
        Date = string.Empty;
        Time = string.Empty;
        Location = string.Empty;
        CoverImageUrl = string.Empty;
        EntityBudget = null!;
    }

    public EntityParty(
        Guid id,
        Guid ownerUserId,
        string name,
        PartyCategory category,
        string date,
        string time,
        string location,
        string coverImageUrl,
        int expectedGuests,
        EntityBudget budget,
        bool isFinalized = false)
    {
        Id = id;
        OwnerUserId = ownerUserId;
        Name = name;
        Category = category;
        Date = date;
        Time = time;
        Location = location;
        CoverImageUrl = coverImageUrl;
        ExpectedGuests = expectedGuests;
        EntityBudget = budget;
        IsFinalized = isFinalized;
    }

    public Guid Id { get; private set; }
    public Guid OwnerUserId { get; private set; }
    public string Name { get; private set; }
    public PartyCategory Category { get; private set; }
    public string Date { get; private set; }
    public string Time { get; private set; }
    public string Location { get; private set; }
    public string CoverImageUrl { get; private set; }
    public int ExpectedGuests { get; private set; }
    public bool IsFinalized { get; private set; }
    public List<EntityPartyTask> Tasks { get; private set; } = [];
    public List<EntityGuest> Guests { get; private set; } = [];
    public EntityBudget EntityBudget { get; private set; }
    public EntityUser Owner { get; private set; } = null!;

    public void AddTask(EntityPartyTask task)
    {
        Tasks.Insert(0, task);
    }

    public void AddGuest(EntityGuest guest)
    {
        Guests.Insert(0, guest);
    }

    public bool ToggleTask(Guid taskId)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task is null) return false;
        task.Toggle();
        return true;
    }

    public bool UpdateTaskStatus(Guid taskId, string status)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task is null) return false;
        task.UpdateStatus(status);
        return true;
    }

    public bool UpdateTask(Guid taskId, string title, string assignee, string description, string status)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task is null) return false;
        task.UpdateDetails(title, assignee, description, status);
        return true;
    }

    public void AddBudgetItem(EntityBudgetItem item)
    {
        EntityBudget.AddItem(item);
    }

    public void UpdateDetails(
        string name,
        PartyCategory category,
        string date,
        string time,
        string location,
        string coverImageUrl,
        int expectedGuests,
        decimal? estimatedBudget,
        bool isFinalized)
    {
        Name = name;
        Category = category;
        Date = date;
        Time = time;
        Location = location;
        CoverImageUrl = coverImageUrl;
        ExpectedGuests = expectedGuests;
        EntityBudget.UpdateEstimated(estimatedBudget);
        IsFinalized = isFinalized;
    }

    public bool CanBeEditedOn(DateOnly referenceDate)
    {
        if (!TryGetEventDate(out var eventDate)) return true;
        return referenceDate <= eventDate;
    }

    public bool IsFinalizedOn(DateOnly referenceDate)
    {
        return IsFinalized || IsPastOn(referenceDate);
    }

    public bool FinalizeIfPast(DateOnly referenceDate)
    {
        if (IsFinalized || !IsPastOn(referenceDate)) return false;
        IsFinalized = true;
        return true;
    }

    public void EnsureEditableOn(DateOnly referenceDate)
    {
        FinalizeIfPast(referenceDate);
        if (!CanBeEditedOn(referenceDate))
            throw new DomainException("A festa nao pode mais ser editada apos a data de realizacao.");
    }

    public void EnsureAcceptingChangesOn(DateOnly referenceDate)
    {
        FinalizeIfPast(referenceDate);
        if (IsFinalized)
            throw new DomainException("A festa esta finalizada e nao aceita novas alteracoes.");
        if (!CanBeEditedOn(referenceDate))
            throw new DomainException("A festa nao pode mais ser editada apos a data de realizacao.");
    }

    private bool IsPastOn(DateOnly referenceDate)
    {
        return TryGetEventDate(out var eventDate) && eventDate < referenceDate;
    }

    private bool TryGetEventDate(out DateOnly eventDate)
    {
        return DateOnly.TryParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out eventDate)
               || DateOnly.TryParse(Date, CultureInfo.InvariantCulture, DateTimeStyles.None, out eventDate);
    }
}
