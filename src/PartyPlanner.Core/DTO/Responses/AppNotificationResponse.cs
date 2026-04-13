namespace PartyPlanner.Core.DTO.Responses;

public sealed record AppNotificationResponse(
    Guid Id,
    string Title,
    string Message,
    string Type,
    bool IsRead,
    DateTime CreatedAtUtc
);
