using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.DeleteBudgetItem;

public sealed class DeleteBudgetItemCommandHandler(
    IPartyRepository partyRepository,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<DeleteBudgetItemCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(DeleteBudgetItemCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);
        await partyRepository.DeleteBudgetItemAsync(party.Id, request.BudgetItemId, cancellationToken);
        await publisher.Publish(new BudgetItemDeletedEvent(request.OwnerUserId, party.Name), cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }
}
