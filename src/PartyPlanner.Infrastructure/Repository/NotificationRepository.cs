using Microsoft.EntityFrameworkCore;
using PartyPlanner.Core.Interfaces.Repositories;
using PartyPlanner.Core.Entities;
using PartyPlanner.Infrastructure.Data;

namespace PartyPlanner.Infrastructure.Repository;

public sealed class NotificationRepository(PartyPlannerDbContext dbContext) : INotificationRepository
{
    public async Task<IReadOnlyCollection<EntityAppNotification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.AppNotifications
            .Where(notification => notification.UserId == userId)
            .OrderByDescending(notification => notification.CreatedAtUtc)
            .ToArrayAsync(cancellationToken);
    }

    public Task<EntityAppNotification?> GetByIdAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default)
    {
        return dbContext.AppNotifications
            .FirstOrDefaultAsync(
                notification => notification.Id == notificationId && notification.UserId == userId,
                cancellationToken);
    }

    public Task<int> DeleteAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return dbContext.AppNotifications
            .Where(notification => notification.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task AddAsync(EntityAppNotification notification, CancellationToken cancellationToken = default)
    {
        return dbContext.AppNotifications.AddAsync(notification, cancellationToken).AsTask();
    }
}
