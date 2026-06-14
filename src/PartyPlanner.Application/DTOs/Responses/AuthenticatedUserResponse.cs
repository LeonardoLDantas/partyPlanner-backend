namespace PartyPlanner.Application.DTOs.Responses;

public sealed record AuthenticatedUserResponse(
    Guid Id,
    string Name,
    string Email
);
