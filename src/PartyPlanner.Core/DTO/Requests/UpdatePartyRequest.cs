using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.DTO.Requests;

public sealed record UpdatePartyRequest(
    string Name,
    PartyCategory? Category,
    string? Date,
    string? Time,
    string? Location,
    string? CoverImageUrl,
    int? ExpectedGuests,
    decimal? EstimatedBudget
);
