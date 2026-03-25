namespace PartyPlanner.Core.DTO.Requests;

public sealed record CreatePartyRequest(
    string Name,
    string? Category,
    string? Date,
    string? Location,
    decimal EstimatedBudget
);
