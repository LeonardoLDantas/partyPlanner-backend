using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.RespondInvitation;

public sealed record RespondInvitationCommand(string Token, string Status) : IRequest<InvitationResponse?>;
