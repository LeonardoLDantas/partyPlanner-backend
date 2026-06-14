using PartyPlanner.Core.Entities;

namespace PartyPlanner.Core.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<IReadOnlyCollection<EntityAppNotification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<EntityAppNotification?> GetByIdAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> DeleteAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(EntityAppNotification notification, CancellationToken cancellationToken = default);
}
