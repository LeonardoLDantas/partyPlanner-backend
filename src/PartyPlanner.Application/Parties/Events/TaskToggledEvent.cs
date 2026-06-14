using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record TaskToggledEvent(Guid OwnerId) : INotification;
