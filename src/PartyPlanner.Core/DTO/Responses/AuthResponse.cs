namespace PartyPlanner.Core.DTO.Responses;

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    AuthenticatedUserResponse User
);
