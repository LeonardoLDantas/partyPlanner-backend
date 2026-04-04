namespace PartyPlanner.Application.Interface;

public sealed record GoogleUserInfo(
    string Subject,
    string Email,
    string Name,
    bool EmailVerified
);
