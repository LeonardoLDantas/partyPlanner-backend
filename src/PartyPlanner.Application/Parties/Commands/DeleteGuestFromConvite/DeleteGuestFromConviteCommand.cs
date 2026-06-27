using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.DeleteGuestFromConvite;

public sealed record DeleteGuestFromConviteCommand(
    Guid OwnerUserId,
    Guid PartyId,
    Guid ConviteId,
    Guid GuestId
) : IRequest<PartyResponse?>;
