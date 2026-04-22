using PartyPlanner.Core.Entities;

namespace PartyPlanner.Application.Interface;

public interface INotificationRepository
{
    Task<IReadOnlyCollection<AppNotification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<AppNotification?> GetByIdAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> DeleteAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(AppNotification notification, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
