using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Enums;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.AddGuestToConvite;

public sealed class AddGuestToConviteCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<AddGuestToConviteCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(AddGuestToConviteCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);

        var convite = party.Convites.FirstOrDefault(c => c.Id == request.ConviteId);
        if (convite is null) return null;

        var guest = new EntityGuest(
            Guid.NewGuid(),
            request.Name.Trim(),
            request.Group ?? GuestGroup.Outros,
            request.Type ?? GuestType.Adulto,
            "Pendente",
            CreateInvitationToken(),
            string.IsNullOrWhiteSpace(request.Email) ? string.Empty : request.Email.Trim(),
            string.IsNullOrWhiteSpace(request.PhoneNumber) ? string.Empty : request.PhoneNumber.Trim()
        );

        await partyRepository.AddGuestToConviteAsync(request.ConviteId, guest, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new GuestAddedEvent(request.OwnerUserId, request.Name.Trim()), cancellationToken);

        var updated = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updated ?? party).ToResponse();
    }

    private static string CreateInvitationToken() =>
        Convert.ToHexString(Guid.NewGuid().ToByteArray()).ToLowerInvariant();
}
