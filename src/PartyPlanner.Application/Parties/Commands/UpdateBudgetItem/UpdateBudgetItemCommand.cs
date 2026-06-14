using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.UpdateBudgetItem;

public sealed record UpdateBudgetItemCommand(
    Guid OwnerUserId,
    Guid PartyId,
    Guid BudgetItemId,
    decimal Amount,
    bool IsPaid) : IRequest<PartyResponse?>;
