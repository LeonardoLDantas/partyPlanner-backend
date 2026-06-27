using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.UpdateConvite;

public sealed class UpdateConviteCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<UpdateConviteCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(UpdateConviteCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);

        var convite = party.Convites.FirstOrDefault(c => c.Id == request.ConviteId);
        if (convite is null) return null;

        convite.Update(request.Nome.Trim(), request.Observacao, request.Tipo, request.SenhaPresente);
        await unitOfWork.CommitAsync(cancellationToken);

        var updated = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updated ?? party).ToResponse();
    }
}
