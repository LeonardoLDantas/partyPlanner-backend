using MediatR;
using PartyPlanner.Application.Interfaces;

namespace PartyPlanner.Application.Parties.Events.Handlers;

public sealed class PartyNotificationEventHandler(INotificationService notificationService)
    : INotificationHandler<PartyCreatedEvent>,
      INotificationHandler<PartyUpdatedEvent>,
      INotificationHandler<PartyDeletedEvent>,
      INotificationHandler<TaskAddedEvent>,
      INotificationHandler<TaskUpdatedEvent>,
      INotificationHandler<TaskDeletedEvent>,
      INotificationHandler<TaskToggledEvent>,
      INotificationHandler<GuestAddedEvent>,
      INotificationHandler<GuestRemovedEvent>,
      INotificationHandler<BudgetItemAddedEvent>,
      INotificationHandler<BudgetItemUpdatedEvent>,
      INotificationHandler<BudgetItemDeletedEvent>,
      INotificationHandler<InvitationRespondedEvent>
{
    public Task Handle(PartyCreatedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Nova festa criada", $"A festa \"{n.PartyName}\" foi criada com sucesso.", "party", ct);

    public Task Handle(PartyUpdatedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Festa atualizada", $"A festa \"{n.PartyName}\" foi atualizada.", "party", ct);

    public Task Handle(PartyDeletedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Festa excluida", $"A festa \"{n.PartyName}\" foi excluida.", "party", ct);

    public Task Handle(TaskAddedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Tarefa adicionada", $"A tarefa \"{n.TaskTitle}\" foi adicionada em \"{n.PartyName}\".", "task", ct);

    public Task Handle(TaskUpdatedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Tarefa movida", $"Uma tarefa foi movida para \"{n.NewStatus}\" em \"{n.PartyName}\".", "task", ct);

    public Task Handle(TaskDeletedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Tarefa removida", $"Uma tarefa foi removida de \"{n.PartyName}\".", "task", ct);

    public Task Handle(TaskToggledEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Tarefa atualizada", "O status de uma tarefa foi atualizado.", "task", ct);

    public Task Handle(GuestAddedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Convidado adicionado", $"\"{n.GuestName}\" foi adicionado a lista de convidados.", "guest", ct);

    public Task Handle(GuestRemovedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Convidado removido", $"Um convidado foi removido de \"{n.PartyName}\".", "guest", ct);

    public Task Handle(BudgetItemAddedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Despesa adicionada", $"A despesa \"{n.Label}\" foi registrada no evento \"{n.PartyName}\".", "budget", ct);

    public Task Handle(BudgetItemUpdatedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Despesa atualizada", $"Uma despesa do evento \"{n.PartyName}\" foi atualizada.", "budget", ct);

    public Task Handle(BudgetItemDeletedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Despesa removida", $"Uma despesa do evento \"{n.PartyName}\" foi removida.", "budget", ct);

    public Task Handle(InvitationRespondedEvent n, CancellationToken ct) =>
        notificationService.CreateAsync(n.OwnerId, "Resposta de convite", $"\"{n.GuestName}\" marcou presenca como {n.Status} em \"{n.PartyName}\".", "guest", ct);
}
