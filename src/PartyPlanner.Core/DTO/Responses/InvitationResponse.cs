namespace PartyPlanner.Core.DTO.Responses;

public sealed record InvitationResponse(
    string Token,
    string GuestName,
    string GuestStatus,
    string PartyName,
    string PartyDate,
    string PartyTime,
    string PartyLocation,
    string PartyCoverImageUrl
);
