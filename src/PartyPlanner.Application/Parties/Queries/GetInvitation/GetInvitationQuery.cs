using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Queries.GetInvitation;

public sealed record GetInvitationQuery(string Token) : IRequest<InvitationResponse?>;
