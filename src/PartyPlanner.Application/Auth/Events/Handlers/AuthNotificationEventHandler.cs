using MediatR;
using PartyPlanner.Application.Interfaces;

namespace PartyPlanner.Application.Auth.Events.Handlers;

public sealed class AuthNotificationEventHandler(INotificationService notificationService)
    : INotificationHandler<UserRegisteredEvent>
{
    public Task Handle(UserRegisteredEvent notification, CancellationToken ct) =>
        notificationService.CreateAsync(notification.UserId, "Conta criada",
            "Sua conta foi criada com sucesso. Bem-vindo ao Party Planner.", "account", ct);
}
