namespace PartyPlanner.Core.DTO.Requests;

public sealed record CreateGuestRequest(
    string Name,
    string? Group,
    string? Status
);
