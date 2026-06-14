using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.DeleteBudgetItem;

public sealed record DeleteBudgetItemCommand(Guid OwnerUserId, Guid PartyId, Guid BudgetItemId) : IRequest<PartyResponse?>;
