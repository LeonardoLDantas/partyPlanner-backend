using MediatR;
namespace PartyPlanner.Application.Parties.Events;
public sealed record InvitationRespondedEvent(Guid OwnerId, string GuestName, string Status, string PartyName) : INotification;
