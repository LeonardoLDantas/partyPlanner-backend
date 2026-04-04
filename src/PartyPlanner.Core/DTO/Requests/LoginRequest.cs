namespace PartyPlanner.Core.DTO.Requests;

public sealed record LoginRequest(
    string Email,
    string Password
);
