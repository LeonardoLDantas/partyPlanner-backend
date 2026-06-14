using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record PartyCreatedEvent(Guid OwnerId, string PartyName) : INotification;
