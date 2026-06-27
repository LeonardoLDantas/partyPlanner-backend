using PartyPlanner.Core.Enums;

namespace PartyPlanner.Application.DTOs.Responses;

public sealed record ConviteResponse(
    Guid Id,
    string Nome,
    string Observacao,
    InviteType Tipo,
    string SenhaPresente,
    IReadOnlyCollection<ConviteSenhaResponse> Senhas,
    IReadOnlyCollection<GuestResponse> Guests
);
