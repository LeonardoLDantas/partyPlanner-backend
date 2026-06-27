using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.Parties.Commands.CreateConvite;

public sealed record CreateConviteCommand(
    Guid OwnerUserId,
    Guid PartyId,
    string Nome,
    string? Observacao,
    InviteType Tipo,
    int QuantidadeSenhas,
    string? SenhaPresente
) : IRequest<PartyResponse?>;
