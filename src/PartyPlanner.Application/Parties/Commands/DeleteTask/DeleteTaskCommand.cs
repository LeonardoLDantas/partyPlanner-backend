using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.DeleteTask;

public sealed record DeleteTaskCommand(Guid OwnerUserId, Guid PartyId, Guid TaskId) : IRequest<PartyResponse?>;
