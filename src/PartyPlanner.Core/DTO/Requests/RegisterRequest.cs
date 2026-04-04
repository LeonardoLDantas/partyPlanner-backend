namespace PartyPlanner.Core.DTO.Requests;

public sealed record RegisterRequest(
    string Name,
    string Email,
    string Password
);
