using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Core.Enums;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.UpdateGuest;

public sealed class UpdateGuestCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<UpdateGuestCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(UpdateGuestCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);

        var convite = party.Convites.FirstOrDefault(c => c.Id == request.ConviteId);
        if (convite is null) return null;

        var guest = convite.Guests.FirstOrDefault(g => g.Id == request.GuestId);
        if (guest is null) return null;

        guest.UpdateDetails(
            request.Name.Trim(),
            request.Group ?? GuestGroup.Outros,
            request.Type ?? GuestType.Adulto,
            string.IsNullOrWhiteSpace(request.Email) ? string.Empty : request.Email.Trim(),
            string.IsNullOrWhiteSpace(request.PhoneNumber) ? string.Empty : request.PhoneNumber.Trim()
        );

        await unitOfWork.CommitAsync(cancellationToken);

        var updated = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updated ?? party).ToResponse();
    }
}
