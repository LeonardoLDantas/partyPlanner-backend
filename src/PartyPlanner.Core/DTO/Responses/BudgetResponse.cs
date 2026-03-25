namespace PartyPlanner.Core.DTO.Responses;

public sealed record BudgetResponse(
    decimal Estimated,
    decimal Spent,
    IReadOnlyCollection<BudgetItemResponse> Items
);
