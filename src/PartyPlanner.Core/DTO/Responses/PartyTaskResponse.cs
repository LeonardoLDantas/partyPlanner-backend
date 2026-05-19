namespace PartyPlanner.Core.DTO.Responses;

public sealed record PartyTaskResponse(
    Guid Id,
    string Title,
    string Assignee,
    string DueDate,
    string Description,
    string Status,
    bool Done
);
