using PartyPlanner.Core.DTO.Responses;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Core.Extensions;

public static class NotificationMappingExtensions
{
    public static AppNotificationResponse ToResponse(this AppNotification notification)
    {
        return new AppNotificationResponse(
            notification.Id,
            notification.Title,
            notification.Message,
            notification.Type,
            notification.IsRead,
            notification.CreatedAtUtc);
    }
}
