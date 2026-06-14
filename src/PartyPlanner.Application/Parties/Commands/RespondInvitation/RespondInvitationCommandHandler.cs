using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.RespondInvitation;

public sealed class RespondInvitationCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<RespondInvitationCommand, InvitationResponse?>
{
    public async Task<InvitationResponse?> Handle(RespondInvitationCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByInvitationTokenAsync(request.Token, cancellationToken);
        var guest = party?.Guests.FirstOrDefault(g => g.InvitationToken == request.Token);
        if (party is null || guest is null) return null;

        var status = NormalizeInvitationStatus(request.Status);
        guest.UpdateStatus(status);
        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new InvitationRespondedEvent(party.OwnerUserId, guest.Name, status, party.Name), cancellationToken);

        return ToInvitationResponse(party, guest);
    }

    private static string NormalizeInvitationStatus(string status) =>
        status.Trim().Equals("Recusou", StringComparison.OrdinalIgnoreCase) ? "Recusou" : "Confirmado";

    private static InvitationResponse ToInvitationResponse(EntityParty party, EntityGuest guest) =>
        new(guest.InvitationToken, guest.Name, guest.Status, party.Name, party.Date, party.Time, party.Location, party.CoverImageUrl);
}
