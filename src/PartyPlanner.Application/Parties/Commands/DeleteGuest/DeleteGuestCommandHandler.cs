// Obsoleto: use DeleteGuestFromConviteCommand. Mantido apenas para não quebrar o build.
using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.DeleteGuest;

public sealed class DeleteGuestCommandHandler : IRequestHandler<DeleteGuestCommand, PartyResponse?>
{
    public Task<PartyResponse?> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
        => Task.FromResult<PartyResponse?>(null);
}
