using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record GuestAddedEvent(Guid OwnerId, string GuestName) : INotification;
