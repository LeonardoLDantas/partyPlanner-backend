using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Enums;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.AddBudgetItem;

public sealed class AddBudgetItemCommandHandler(
    IPartyRepository partyRepository,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<AddBudgetItemCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(AddBudgetItemCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);

        var budgetItem = new EntityBudgetItem(
            Guid.NewGuid(),
            request.Label.Trim(),
            request.Category ?? ExpenseCategory.Outros,
            request.Amount,
            request.IsPaid);

        await partyRepository.AddBudgetItemAsync(party.Id, budgetItem, cancellationToken);
        await publisher.Publish(new BudgetItemAddedEvent(request.OwnerUserId, request.Label.Trim(), party.Name), cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }
}
