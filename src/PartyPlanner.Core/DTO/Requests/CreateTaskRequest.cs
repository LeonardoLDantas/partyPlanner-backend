namespace PartyPlanner.Core.DTO.Requests;

public sealed record CreateTaskRequest(
    string Title,
    string? Assignee
);
