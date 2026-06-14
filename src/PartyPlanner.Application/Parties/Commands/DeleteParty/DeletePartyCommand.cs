using MediatR;

namespace PartyPlanner.Application.Parties.Commands.DeleteParty;

public sealed record DeletePartyCommand(Guid OwnerUserId, Guid PartyId) : IRequest<bool>;
