using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.AddTask;

public sealed record AddTaskCommand(
    Guid OwnerUserId,
    Guid PartyId,
    string Title,
    string? Assignee,
    string? DueDate,
    string? Description,
    string? Status) : IRequest<PartyResponse?>;
