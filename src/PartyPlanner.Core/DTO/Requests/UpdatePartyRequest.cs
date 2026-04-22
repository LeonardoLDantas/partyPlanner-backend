using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.DTO.Requests;

public sealed record UpdatePartyRequest(
    string Name,
    PartyCategory? Category,
    string? Date,
    string? Location,
    decimal EstimatedBudget
);
