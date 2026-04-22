using PartyPlanner.Core.DTO.Responses;

namespace PartyPlanner.Application.Interface;

public interface INotificationService
{
    Task<IReadOnlyCollection<AppNotificationResponse>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<AppNotificationResponse?> MarkAsReadAsync(Guid userId, Guid notificationId, CancellationToken cancellationToken = default);
    Task<int> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> ClearAllAsync(Guid userId, CancellationToken cancellationToken = default);
    Task CreateAsync(Guid userId, string title, string message, string type, CancellationToken cancellationToken = default);
}
