using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Responses;

public sealed record BudgetItemResponse(
    Guid Id,
    string Label,
    ExpenseCategory Category,
    decimal Amount,
    bool IsPaid
);
