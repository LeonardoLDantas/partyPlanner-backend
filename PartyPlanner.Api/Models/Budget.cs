namespace PartyPlanner.Api.Models;

public sealed class Budget
{
    public decimal Estimated { get; set; }
    public decimal Spent { get; set; }
    public List<BudgetItem> Items { get; set; } = [];
}
