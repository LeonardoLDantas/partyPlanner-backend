using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.DTO.Requests;

public sealed record CreateBudgetItemRequest(
    string Label,
    ExpenseCategory? Category,
    decimal Amount,
    bool IsPaid
);
