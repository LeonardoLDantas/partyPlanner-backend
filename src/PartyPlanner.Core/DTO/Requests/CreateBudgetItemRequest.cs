namespace PartyPlanner.Core.DTO.Requests;

public sealed record CreateBudgetItemRequest(
    string Label,
    string? Category,
    decimal Amount
);
