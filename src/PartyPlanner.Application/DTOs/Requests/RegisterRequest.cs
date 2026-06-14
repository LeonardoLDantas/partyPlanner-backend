namespace PartyPlanner.Application.DTOs.Requests;

public sealed record RegisterRequest(
    string Name,
    string Email,
    string Password
);
