using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record BudgetItemUpdatedEvent(Guid OwnerId, string PartyName) : INotification;
