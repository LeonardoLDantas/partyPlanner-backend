using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.DeleteConvite;

public sealed class DeleteConviteCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteConviteCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(DeleteConviteCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        await partyRepository.DeleteConviteAsync(request.PartyId, request.ConviteId, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        var updated = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updated ?? party).ToResponse();
    }
}
