using PartyPlanner.Application.Interfaces;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Services;

public sealed class NotificationService(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : INotificationService
{
    public async Task CreateAsync(Guid userId, string title, string message, string type, CancellationToken cancellationToken = default)
    {
        var notification = new EntityAppNotification(
            Guid.NewGuid(),
            userId,
            title.Trim(),
            message.Trim(),
            type.Trim().ToLowerInvariant());

        await notificationRepository.AddAsync(notification, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
