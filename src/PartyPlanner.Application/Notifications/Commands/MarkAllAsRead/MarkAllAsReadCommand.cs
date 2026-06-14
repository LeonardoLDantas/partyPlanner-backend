using MediatR;

namespace PartyPlanner.Application.Notifications.Commands.MarkAllAsRead;

public sealed record MarkAllAsReadCommand(Guid UserId) : IRequest<int>;
