using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.Parties.Commands.UpdateParty;

public sealed record UpdatePartyCommand(
    Guid OwnerUserId,
    Guid PartyId,
    string Name,
    PartyCategory? Category,
    string? Date,
    string? Time,
    string? Location,
    string? CoverImageUrl,
    int? ExpectedGuests,
    decimal? EstimatedBudget,
    bool? IsFinalized) : IRequest<PartyResponse?>;
