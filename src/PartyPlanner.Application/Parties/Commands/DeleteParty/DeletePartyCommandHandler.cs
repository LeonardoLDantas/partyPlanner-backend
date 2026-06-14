using MediatR;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.DeleteParty;

public sealed class DeletePartyCommandHandler(
    IPartyRepository partyRepository,
    IPublisher publisher) : IRequestHandler<DeletePartyCommand, bool>
{
    public async Task<bool> Handle(DeletePartyCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return false;

        if (!await partyRepository.DeleteAsync(request.PartyId, request.OwnerUserId, cancellationToken))
            return false;

        await publisher.Publish(new PartyDeletedEvent(request.OwnerUserId, party.Name), cancellationToken);
        return true;
    }
}
