using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.Parties.Commands.AddGuest;

public sealed record AddGuestCommand(
    Guid OwnerUserId,
    Guid PartyId,
    string Name,
    string? Group,
    GuestType? Type,
    string? Email,
    string? PhoneNumber) : IRequest<PartyResponse?>;
