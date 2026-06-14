using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.DeleteGuest;

public sealed record DeleteGuestCommand(Guid OwnerUserId, Guid PartyId, Guid GuestId) : IRequest<PartyResponse?>;
