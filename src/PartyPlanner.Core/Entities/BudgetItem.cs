namespace PartyPlanner.Core.Entities;

public sealed class BudgetItem
{
    private BudgetItem()
    {
    }

    public BudgetItem(Guid id, string label, string category, decimal amount)
    {
        Id = id;
        Label = label;
        Category = category;
        Amount = amount;
    }

    public Guid Id { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
}
