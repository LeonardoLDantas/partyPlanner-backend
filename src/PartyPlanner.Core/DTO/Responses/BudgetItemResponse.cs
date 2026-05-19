using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.DTO.Responses;

public sealed record BudgetItemResponse(
    Guid Id,
    string Label,
    ExpenseCategory Category,
    decimal Amount
);
