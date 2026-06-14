using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record BudgetItemAddedEvent(Guid OwnerId, string Label, string PartyName) : INotification;
