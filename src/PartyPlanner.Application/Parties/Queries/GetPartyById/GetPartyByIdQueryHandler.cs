using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Queries.GetPartyById;

public sealed class GetPartyByIdQueryHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<GetPartyByIdQuery, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(GetPartyByIdQuery request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.Id, request.OwnerUserId, cancellationToken);
        if (party?.FinalizeIfPast(dateTimeProvider.Today) == true)
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }

        return party?.ToResponse();
    }
}
