using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record TaskUpdatedEvent(Guid OwnerId, string NewStatus, string PartyName) : INotification;
