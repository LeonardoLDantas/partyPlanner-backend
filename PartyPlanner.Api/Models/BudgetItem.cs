namespace PartyPlanner.Api.Models;

public sealed class BudgetItem
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
