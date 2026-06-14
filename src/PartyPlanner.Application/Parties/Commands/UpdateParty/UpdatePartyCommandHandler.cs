using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Enums;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.UpdateParty;

public sealed class UpdatePartyCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<UpdatePartyCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(UpdatePartyCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureEditableOn(dateTimeProvider.Today);
        party.UpdateDetails(
            request.Name.Trim(),
            request.Category ?? PartyCategory.Outros,
            string.IsNullOrWhiteSpace(request.Date) ? "Data a definir" : request.Date.Trim(),
            string.IsNullOrWhiteSpace(request.Time) ? "19:00" : request.Time.Trim(),
            string.IsNullOrWhiteSpace(request.Location) ? "Local a definir" : request.Location.Trim(),
            string.IsNullOrWhiteSpace(request.CoverImageUrl) ? party.CoverImageUrl : request.CoverImageUrl.Trim(),
            Math.Max(request.ExpectedGuests ?? party.ExpectedGuests, 0),
            request.EstimatedBudget,
            request.IsFinalized ?? party.IsFinalized);

        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new PartyUpdatedEvent(request.OwnerUserId, party.Name), cancellationToken);

        return party.ToResponse();
    }
}
