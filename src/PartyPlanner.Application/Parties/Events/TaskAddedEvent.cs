using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record TaskAddedEvent(Guid OwnerId, string TaskTitle, string PartyName) : INotification;
