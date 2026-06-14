using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Requests;

public sealed record CreateBudgetItemRequest(
    string Label,
    ExpenseCategory? Category,
    decimal Amount,
    bool IsPaid
);
