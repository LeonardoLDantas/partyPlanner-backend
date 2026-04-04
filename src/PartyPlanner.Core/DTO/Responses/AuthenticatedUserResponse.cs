namespace PartyPlanner.Core.DTO.Responses;

public sealed record AuthenticatedUserResponse(
    Guid Id,
    string Name,
    string Email
);
