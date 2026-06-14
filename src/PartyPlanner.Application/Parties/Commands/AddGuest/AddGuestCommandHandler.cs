using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Enums;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.AddGuest;

public sealed class AddGuestCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<AddGuestCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(AddGuestCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);

        var guest = new EntityGuest(
            Guid.NewGuid(),
            request.Name.Trim(),
            string.IsNullOrWhiteSpace(request.Group) ? "Geral" : request.Group.Trim(),
            request.Type ?? GuestType.Adulto,
            "Pendente",
            CreateInvitationToken(),
            string.IsNullOrWhiteSpace(request.Email) ? string.Empty : request.Email.Trim(),
            string.IsNullOrWhiteSpace(request.PhoneNumber) ? string.Empty : request.PhoneNumber.Trim()
        );

        await partyRepository.AddGuestAsync(party.Id, guest, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new GuestAddedEvent(request.OwnerUserId, request.Name.Trim()), cancellationToken);

        // TODO: habilitar após verificar domínio no Resend (resend.com/domains)
        // if (!string.IsNullOrWhiteSpace(guest.Email))
        // {
        //     await mediator.Send(new SendInvitationCommand(request.OwnerUserId, party.Id, guest.Id), cancellationToken);
        // }

        var updatedParty = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }

    private static string CreateInvitationToken() =>
        Convert.ToHexString(Guid.NewGuid().ToByteArray()).ToLowerInvariant();
}
