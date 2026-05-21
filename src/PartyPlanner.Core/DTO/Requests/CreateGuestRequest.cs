using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.DTO.Requests;

public sealed record CreateGuestRequest(
    string Name,
    string? Group,
    GuestType? Type,
    string? Email,
    string? PhoneNumber
);
