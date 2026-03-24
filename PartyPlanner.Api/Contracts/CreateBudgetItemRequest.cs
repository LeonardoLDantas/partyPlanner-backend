namespace PartyPlanner.Api.Contracts;

public sealed record CreateBudgetItemRequest(
    string Label,
    string? Category,
    decimal Amount
);
