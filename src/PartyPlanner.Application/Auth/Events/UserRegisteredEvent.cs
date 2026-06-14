using MediatR;
namespace PartyPlanner.Application.Auth.Events;
public sealed record UserRegisteredEvent(Guid UserId) : INotification;
