using MediatR;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Notifications.Commands.MarkAllAsRead;

public sealed class MarkAllAsReadCommandHandler(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<MarkAllAsReadCommand, int>
{
    public async Task<int> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        var notifications = await notificationRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        var unread = notifications.Where(n => !n.IsRead).ToArray();

        foreach (var n in unread)
            n.MarkAsRead();

        if (unread.Length > 0)
            await unitOfWork.CommitAsync(cancellationToken);

        return unread.Length;
    }
}
