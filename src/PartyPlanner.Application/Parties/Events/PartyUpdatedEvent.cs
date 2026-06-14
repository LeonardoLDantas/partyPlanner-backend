using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record PartyUpdatedEvent(Guid OwnerId, string PartyName) : INotification;
