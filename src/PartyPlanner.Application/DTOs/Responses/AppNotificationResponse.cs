namespace PartyPlanner.Application.DTOs.Responses;

public sealed record AppNotificationResponse(
    Guid Id,
    string Title,
    string Message,
    string Type,
    bool IsRead,
    DateTime CreatedAtUtc
);
