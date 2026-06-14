using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record TaskDeletedEvent(Guid OwnerId, string PartyName) : INotification;
