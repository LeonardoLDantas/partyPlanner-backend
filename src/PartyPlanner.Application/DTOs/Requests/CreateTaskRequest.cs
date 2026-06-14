namespace PartyPlanner.Application.DTOs.Requests;

public sealed record CreateTaskRequest(
    string Title,
    string? Assignee,
    string? DueDate,
    string? Description,
    string? Status
);
