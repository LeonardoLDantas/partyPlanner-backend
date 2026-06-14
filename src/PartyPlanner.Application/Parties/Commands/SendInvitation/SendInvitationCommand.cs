using MediatR;

namespace PartyPlanner.Application.Parties.Commands.SendInvitation;

public sealed record SendInvitationCommand(
    Guid OwnerUserId,
    Guid PartyId,
    Guid GuestId) : IRequest;
