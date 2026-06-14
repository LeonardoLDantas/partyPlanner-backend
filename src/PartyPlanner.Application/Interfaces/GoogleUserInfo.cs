namespace PartyPlanner.Application.Interfaces;

public sealed record GoogleUserInfo(
    string Subject,
    string Email,
    string Name,
    bool EmailVerified
);
