using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record GuestRemovedEvent(Guid OwnerId, string PartyName) : INotification;
