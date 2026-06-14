using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.DeleteGuest;

public sealed class DeleteGuestCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<DeleteGuestCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);
        if (party.Guests.All(g => g.Id != request.GuestId)) return null;

        await partyRepository.DeleteGuestAsync(party.Id, request.GuestId, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new GuestRemovedEvent(request.OwnerUserId, party.Name), cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }
}
