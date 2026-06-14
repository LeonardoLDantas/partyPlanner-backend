using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Queries.GetPartyById;

public sealed record GetPartyByIdQuery(Guid Id, Guid OwnerUserId) : IRequest<PartyResponse?>;
