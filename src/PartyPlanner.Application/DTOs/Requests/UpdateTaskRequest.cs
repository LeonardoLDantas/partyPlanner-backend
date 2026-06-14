namespace PartyPlanner.Application.DTOs.Requests;

public sealed record UpdateTaskRequest(
    string? Title,
    string? Assignee,
    string? Description,
    string? Status
);
