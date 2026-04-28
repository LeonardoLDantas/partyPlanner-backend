using PartyPlanner.Application.Interface;
using PartyPlanner.Core.DTO.Responses;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Extensions;

namespace PartyPlanner.Application.Services;

public sealed class NotificationService(INotificationRepository notificationRepository) : INotificationService
{
    public async Task<IReadOnlyCollection<AppNotificationResponse>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = await notificationRepository.GetByUserIdAsync(userId, cancellationToken);
        return notifications.Select(notification => notification.ToResponse()).ToArray();
    }

    public async Task<AppNotificationResponse?> MarkAsReadAsync(Guid userId, Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await notificationRepository.GetByIdAsync(notificationId, userId, cancellationToken);
        if (notification is null)
        {
            return null;
        }

        notification.MarkAsRead();
        await notificationRepository.SaveChangesAsync(cancellationToken);
        return notification.ToResponse();
    }

    public async Task<int> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = await notificationRepository.GetByUserIdAsync(userId, cancellationToken);
        var unreadNotifications = notifications.Where(notification => !notification.IsRead).ToArray();

        foreach (var notification in unreadNotifications)
        {
            notification.MarkAsRead();
        }

        if (unreadNotifications.Length > 0)
        {
            await notificationRepository.SaveChangesAsync(cancellationToken);
        }

        return unreadNotifications.Length;
    }

    public Task<int> ClearAllAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return notificationRepository.DeleteAllByUserIdAsync(userId, cancellationToken);
    }

    public async Task CreateAsync(Guid userId, string title, string message, string type, CancellationToken cancellationToken = default)
    {
        var notification = new AppNotification(
            Guid.NewGuid(),
            userId,
            title.Trim(),
            message.Trim(),
            type.Trim().ToLowerInvariant());

        await notificationRepository.AddAsync(notification, cancellationToken);
        await notificationRepository.SaveChangesAsync(cancellationToken);
    }
}
