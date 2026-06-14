using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Notifications.Commands.MarkAsRead;

public sealed class MarkAsReadCommandHandler(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<MarkAsReadCommand, AppNotificationResponse?>
{
    public async Task<AppNotificationResponse?> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.GetByIdAsync(request.NotificationId, request.UserId, cancellationToken);
        if (notification is null) return null;

        notification.MarkAsRead();
        await unitOfWork.CommitAsync(cancellationToken);
        return notification.ToResponse();
    }
}
