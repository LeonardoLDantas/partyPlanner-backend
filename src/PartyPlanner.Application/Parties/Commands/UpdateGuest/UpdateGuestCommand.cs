using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.Parties.Commands.UpdateGuest;

public sealed record UpdateGuestCommand(
    Guid OwnerUserId,
    Guid PartyId,
    Guid ConviteId,
    Guid GuestId,
    string Name,
    GuestGroup? Group,
    GuestType? Type,
    string? Email,
    string? PhoneNumber
) : IRequest<PartyResponse?>;
