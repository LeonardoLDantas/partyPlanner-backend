namespace PartyPlanner.Core.DTO.Responses;

public sealed record BudgetItemResponse(
    Guid Id,
    string Label,
    string Category,
    decimal Amount
);
