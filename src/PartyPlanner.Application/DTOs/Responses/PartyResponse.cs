using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Responses;

public sealed record PartyResponse(
    Guid Id,
    Guid OwnerUserId,
    string Name,
    PartyCategory Category,
    string Date,
    string Time,
    string Location,
    string CoverImageUrl,
    int ExpectedGuests,
    bool IsFinalized,
    bool CanEdit,
    IReadOnlyCollection<PartyTaskResponse> Tasks,
    IReadOnlyCollection<ConviteResponse> Convites,
    BudgetResponse Budget
);
