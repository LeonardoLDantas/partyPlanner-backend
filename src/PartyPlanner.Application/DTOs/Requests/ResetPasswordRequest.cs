namespace PartyPlanner.Application.DTOs.Requests;

public sealed record ResetPasswordRequest(string Token, string NewPassword);
