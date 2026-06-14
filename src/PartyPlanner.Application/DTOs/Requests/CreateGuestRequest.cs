using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Requests;

public sealed record CreateGuestRequest(
    string Name,
    string? Group,
    GuestType? Type,
    string? Email,
    string? PhoneNumber
);
