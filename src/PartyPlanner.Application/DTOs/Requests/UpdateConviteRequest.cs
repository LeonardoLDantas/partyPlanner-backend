using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Requests;

public sealed record UpdateConviteRequest(
    string Nome,
    string? Observacao,
    InviteType Tipo,
    string? SenhaPresente
);
