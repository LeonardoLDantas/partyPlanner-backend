namespace PartyPlanner.Application.DTOs.Responses;

public sealed record BudgetResponse(
    decimal? Estimated,
    decimal Spent,
    IReadOnlyCollection<BudgetItemResponse> Items
);
