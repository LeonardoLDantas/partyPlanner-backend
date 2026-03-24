namespace PartyPlanner.Api.Contracts;

public sealed record CreateTaskRequest(
    string Title,
    string? Assignee
);
