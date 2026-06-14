using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.Parties.Commands.AddBudgetItem;

public sealed record AddBudgetItemCommand(
    Guid OwnerUserId,
    Guid PartyId,
    string Label,
    ExpenseCategory? Category,
    decimal Amount,
    bool IsPaid) : IRequest<PartyResponse?>;
