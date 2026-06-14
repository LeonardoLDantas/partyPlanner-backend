using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record PartyDeletedEvent(Guid OwnerId, string PartyName) : INotification;
