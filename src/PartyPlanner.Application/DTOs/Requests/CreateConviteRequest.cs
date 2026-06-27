using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Requests;

public sealed record CreateConviteRequest(
    string Nome,
    string? Observacao,
    InviteType Tipo,
    int QuantidadeSenhas,
    string? SenhaPresente
);
