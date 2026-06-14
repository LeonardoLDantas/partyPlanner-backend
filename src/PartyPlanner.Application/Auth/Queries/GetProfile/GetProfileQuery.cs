using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Auth.Queries.GetProfile;

public sealed record GetProfileQuery(Guid UserId) : IRequest<AuthenticatedUserResponse?>;
