using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record BudgetItemDeletedEvent(Guid OwnerId, string PartyName) : INotification;
