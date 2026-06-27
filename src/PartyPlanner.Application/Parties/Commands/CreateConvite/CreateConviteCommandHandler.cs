using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.CreateConvite;

public sealed class CreateConviteCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<CreateConviteCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(CreateConviteCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);

        var convite = new EntityConvite(
            Guid.NewGuid(),
            request.Nome.Trim(),
            request.Observacao,
            request.Tipo,
            request.SenhaPresente
        );

        var qtd = Math.Max(1, Math.Min(request.QuantidadeSenhas, 100));
        for (var i = 0; i < qtd; i++)
            convite.AddSenha(new EntityConviteSenha(Guid.NewGuid(), GenerateCodigo()));

        await partyRepository.AddConviteAsync(request.PartyId, convite, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        var updated = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updated ?? party).ToResponse();
    }

    private static string GenerateCodigo()
        => Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("+", "").Replace("/", "").Replace("=", "")[..8].ToUpperInvariant();
}
