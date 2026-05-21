using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.Entities;

public sealed class BudgetItem
{
    private BudgetItem()
    {
    }

    public BudgetItem(Guid id, string label, ExpenseCategory category, decimal amount, bool isPaid)
    {
        Id = id;
        Label = label;
        Category = category;
        Amount = amount;
        IsPaid = isPaid;
    }

    public Guid Id { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public ExpenseCategory Category { get; private set; }
    public decimal Amount { get; private set; }
    public bool IsPaid { get; private set; }
}
