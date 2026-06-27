// Obsoleto: use AddGuestToConviteCommand. Mantido apenas para não quebrar o build.
using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Parties.Commands.AddGuest;

public sealed class AddGuestCommandHandler : IRequestHandler<AddGuestCommand, PartyResponse?>
{
    public Task<PartyResponse?> Handle(AddGuestCommand request, CancellationToken cancellationToken)
        => Task.FromResult<PartyResponse?>(null);
}
