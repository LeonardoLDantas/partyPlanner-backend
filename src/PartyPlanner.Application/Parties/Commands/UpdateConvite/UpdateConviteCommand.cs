using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.Parties.Commands.UpdateConvite;

public sealed record UpdateConviteCommand(
    Guid OwnerUserId,
    Guid PartyId,
    Guid ConviteId,
    string Nome,
    string? Observacao,
    InviteType Tipo,
    string? SenhaPresente
) : IRequest<PartyResponse?>;
