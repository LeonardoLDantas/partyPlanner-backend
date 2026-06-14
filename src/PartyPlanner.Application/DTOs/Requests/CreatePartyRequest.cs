using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Requests;

public sealed record CreatePartyRequest(
    string Name,
    PartyCategory? Category,
    string? Date,
    string? Time,
    string? Location,
    string? CoverImageUrl,
    int? ExpectedGuests,
    decimal? EstimatedBudget,
    bool? IsFinalized
);
