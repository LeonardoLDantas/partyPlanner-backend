using MediatR;

namespace PartyPlanner.Application.Notifications.Commands.ClearAll;

public sealed record ClearAllCommand(Guid UserId) : IRequest<int>;
