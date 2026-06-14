using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.ToggleTask;

public sealed class ToggleTaskCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<ToggleTaskCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(ToggleTaskCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);
        if (!party.ToggleTask(request.TaskId)) return null;

        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new TaskToggledEvent(request.OwnerUserId), cancellationToken);

        return party.ToResponse();
    }
}
