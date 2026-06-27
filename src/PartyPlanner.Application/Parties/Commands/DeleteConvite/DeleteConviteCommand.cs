using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.DeleteConvite;

public sealed record DeleteConviteCommand(Guid OwnerUserId, Guid PartyId, Guid ConviteId) : IRequest<PartyResponse?>;
