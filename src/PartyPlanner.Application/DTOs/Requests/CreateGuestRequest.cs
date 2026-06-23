using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Requests;

public sealed record CreateGuestRequest(
    string Name,
    GuestGroup? Group,
    GuestType? Type,
    string? Email,
    string? PhoneNumber
);
