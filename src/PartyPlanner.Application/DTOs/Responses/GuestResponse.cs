using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Responses;

public sealed record GuestResponse(
    Guid Id,
    string Name,
    string Group,
    GuestType Type,
    string Status,
    string InvitationToken,
    string Email,
    string PhoneNumber
);
