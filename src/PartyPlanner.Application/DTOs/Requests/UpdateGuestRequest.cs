using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Requests;

public sealed record UpdateGuestRequest(
    string Name,
    GuestGroup? Group,
    GuestType? Type,
    string? Email,
    string? PhoneNumber
);
