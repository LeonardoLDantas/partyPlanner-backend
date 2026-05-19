namespace PartyPlanner.Core.DTO.Requests;

public sealed record UpdateTaskRequest(
    string? Title,
    string? Assignee,
    string? Description,
    string? Status
);
