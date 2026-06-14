using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Queries.GetInvitation;

public sealed class GetInvitationQueryHandler(
    IPartyRepository partyRepository) : IRequestHandler<GetInvitationQuery, InvitationResponse?>
{
    public async Task<InvitationResponse?> Handle(GetInvitationQuery request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByInvitationTokenAsync(request.Token, cancellationToken);
        var guest = party?.Guests.FirstOrDefault(g => g.InvitationToken == request.Token);
        return party is null || guest is null ? null : ToInvitationResponse(party, guest);
    }

    private static InvitationResponse ToInvitationResponse(EntityParty party, EntityGuest guest) =>
        new(guest.InvitationToken, guest.Name, guest.Status, party.Name, party.Date, party.Time, party.Location, party.CoverImageUrl);
}
