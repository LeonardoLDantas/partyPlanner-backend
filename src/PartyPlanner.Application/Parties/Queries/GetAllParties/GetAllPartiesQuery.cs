using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Queries.GetAllParties;

public sealed record GetAllPartiesQuery(Guid OwnerUserId) : IRequest<IReadOnlyCollection<PartyResponse>>;
