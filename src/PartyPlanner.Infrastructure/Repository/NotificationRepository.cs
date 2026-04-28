using Microsoft.EntityFrameworkCore;
using PartyPlanner.Application.Interface;
using PartyPlanner.Core.Entities;
using PartyPlanner.Infrastructure.Data;

namespace PartyPlanner.Infrastructure.Repository;

public sealed class NotificationRepository(PartyPlannerDbContext dbContext) : INotificationRepository
{
    public async Task<IReadOnlyCollection<AppNotification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.AppNotifications
            .Where(notification => notification.UserId == userId)
            .OrderByDescending(notification => notification.CreatedAtUtc)
            .ToArrayAsync(cancellationToken);
    }

    public Task<AppNotification?> GetByIdAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default)
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

    public Task AddAsync(AppNotification notification, CancellationToken cancellationToken = default)
    {
        return dbContext.AppNotifications.AddAsync(notification, cancellationToken).AsTask();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
