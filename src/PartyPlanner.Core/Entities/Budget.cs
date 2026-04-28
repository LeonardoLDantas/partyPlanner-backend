namespace PartyPlanner.Core.Entities;

public sealed class Budget
{
    private Budget()
    {
        Items = [];
    }

    public Budget(decimal estimated, decimal spent, IReadOnlyCollection<BudgetItem> items)
    {
        Estimated = estimated;
        Spent = spent;
        Items = items.ToList();
    }

    public decimal Estimated { get; private set; }
    public decimal Spent { get; private set; }
    public List<BudgetItem> Items { get; private set; }

    public void UpdateEstimated(decimal estimated)
    {
        Estimated = estimated;
    }

    public void AddItem(BudgetItem item)
    {
        Items.Insert(0, item);
        Spent += item.Amount;
    }
}
