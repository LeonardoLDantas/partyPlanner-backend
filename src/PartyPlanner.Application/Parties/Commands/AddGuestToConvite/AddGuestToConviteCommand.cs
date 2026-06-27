using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.Parties.Commands.AddGuestToConvite;

public sealed record AddGuestToConviteCommand(
    Guid OwnerUserId,
    Guid PartyId,
    Guid ConviteId,
    string Name,
    GuestGroup? Group,
    GuestType? Type,
    string? Email,
    string? PhoneNumber
) : IRequest<PartyResponse?>;
