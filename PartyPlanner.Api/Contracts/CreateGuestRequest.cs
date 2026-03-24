namespace PartyPlanner.Api.Contracts;

public sealed record CreateGuestRequest(
    string Name,
    string? Group,
    string? Status
);
