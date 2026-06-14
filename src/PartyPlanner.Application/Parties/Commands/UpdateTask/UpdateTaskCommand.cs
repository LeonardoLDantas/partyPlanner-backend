using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.UpdateTask;

public sealed record UpdateTaskCommand(
    Guid OwnerUserId,
    Guid PartyId,
    Guid TaskId,
    string? Title,
    string? Assignee,
    string? Description,
    string? Status) : IRequest<PartyResponse?>;
