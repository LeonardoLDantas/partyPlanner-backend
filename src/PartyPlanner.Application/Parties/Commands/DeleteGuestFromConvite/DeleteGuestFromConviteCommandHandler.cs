using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.DeleteGuestFromConvite;

public sealed class DeleteGuestFromConviteCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteGuestFromConviteCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(DeleteGuestFromConviteCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        await partyRepository.DeleteGuestFromConviteAsync(request.ConviteId, request.GuestId, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        var updated = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updated ?? party).ToResponse();
    }
}
