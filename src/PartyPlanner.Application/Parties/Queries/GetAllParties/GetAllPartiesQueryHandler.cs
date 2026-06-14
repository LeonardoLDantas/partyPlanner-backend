using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Queries.GetAllParties;

public sealed class GetAllPartiesQueryHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<GetAllPartiesQuery, IReadOnlyCollection<PartyResponse>>
{
    public async Task<IReadOnlyCollection<PartyResponse>> Handle(GetAllPartiesQuery request, CancellationToken cancellationToken)
    {
        var parties = await partyRepository.GetAllAsync(request.OwnerUserId, cancellationToken);
        var currentDate = dateTimeProvider.Today;
        var anyFinalized = false;
        foreach (var party in parties)
            anyFinalized |= party.FinalizeIfPast(currentDate);

        if (anyFinalized)
            await unitOfWork.CommitAsync(cancellationToken);

        return parties.Select(party => party.ToResponse()).ToArray();
    }
}
