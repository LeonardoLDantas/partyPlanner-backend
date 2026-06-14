namespace PartyPlanner.Application.DTOs.Responses;

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    AuthenticatedUserResponse User
);
