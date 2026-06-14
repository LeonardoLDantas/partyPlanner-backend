using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Notifications.Commands.MarkAsRead;

public sealed record MarkAsReadCommand(Guid UserId, Guid NotificationId) : IRequest<AppNotificationResponse?>;
