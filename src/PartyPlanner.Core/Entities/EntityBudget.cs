namespace PartyPlanner.Core.Entities;

public sealed class EntityBudget
{
    private EntityBudget()
    {
        Items = [];
    }

    public EntityBudget(decimal? estimated, decimal spent, IReadOnlyCollection<EntityBudgetItem> items)
    {
        Estimated = estimated;
        Spent = spent;
        Items = items.ToList();
    }

    public decimal? Estimated { get; private set; }
    public decimal Spent { get; private set; }
    public List<EntityBudgetItem> Items { get; private set; }

    public void UpdateEstimated(decimal? estimated)
    {
        Estimated = estimated;
    }

    public void AddItem(EntityBudgetItem item)
    {
        Items.Insert(0, item);
        Spent += item.Amount;
    }
}
