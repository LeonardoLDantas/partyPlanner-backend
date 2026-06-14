using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.ToggleTask;

public sealed record ToggleTaskCommand(Guid OwnerUserId, Guid PartyId, Guid TaskId) : IRequest<PartyResponse?>;
