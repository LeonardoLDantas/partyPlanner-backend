using MediatR;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Notifications.Commands.ClearAll;

public sealed class ClearAllCommandHandler(
    INotificationRepository notificationRepository) : IRequestHandler<ClearAllCommand, int>
{
    public Task<int> Handle(ClearAllCommand request, CancellationToken cancellationToken) =>
        notificationRepository.DeleteAllByUserIdAsync(request.UserId, cancellationToken);
}
