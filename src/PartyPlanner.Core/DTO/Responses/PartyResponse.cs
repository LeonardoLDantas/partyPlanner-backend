using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.DTO.Responses;

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
    bool CanEdit,
    IReadOnlyCollection<PartyTaskResponse> Tasks,
    IReadOnlyCollection<GuestResponse> Guests,
    BudgetResponse Budget
);
