using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Application.Mappings;

public static class NotificationMappingExtensions
{
    public static AppNotificationResponse ToResponse(this EntityAppNotification notification)
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
