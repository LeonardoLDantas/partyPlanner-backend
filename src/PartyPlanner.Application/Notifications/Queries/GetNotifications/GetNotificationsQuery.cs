using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Notifications.Queries.GetNotifications;

public sealed record GetNotificationsQuery(Guid UserId) : IRequest<IReadOnlyCollection<AppNotificationResponse>>;
