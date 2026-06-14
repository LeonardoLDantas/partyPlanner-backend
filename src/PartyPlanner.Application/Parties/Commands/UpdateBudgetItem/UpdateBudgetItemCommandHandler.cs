using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.UpdateBudgetItem;

public sealed class UpdateBudgetItemCommandHandler(
    IPartyRepository partyRepository,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<UpdateBudgetItemCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(UpdateBudgetItemCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);
        await partyRepository.UpdateBudgetItemAsync(party.Id, request.BudgetItemId, request.Amount, request.IsPaid, cancellationToken);
        await publisher.Publish(new BudgetItemUpdatedEvent(request.OwnerUserId, party.Name), cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }
}
