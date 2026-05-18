namespace PartyPlanner.Core.DTO.Responses;

public sealed record GuestResponse(
    Guid Id,
    string Name,
    string Group,
    string Status,
    string InvitationToken,
    string Email,
    string PhoneNumber
);
