using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Notifications.Queries.GetNotifications;

public sealed class GetNotificationsQueryHandler(
    INotificationRepository notificationRepository) : IRequestHandler<GetNotificationsQuery, IReadOnlyCollection<AppNotificationResponse>>
{
    public async Task<IReadOnlyCollection<AppNotificationResponse>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await notificationRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        return notifications.Select(n => n.ToResponse()).ToArray();
    }
}
