namespace PartyPlanner.Core.DTO.Responses;

public sealed record PartyResponse(
    Guid Id,
    string Name,
    string Category,
    string Date,
    string Location,
    IReadOnlyCollection<PartyTaskResponse> Tasks,
    IReadOnlyCollection<GuestResponse> Guests,
    BudgetResponse Budget
);
