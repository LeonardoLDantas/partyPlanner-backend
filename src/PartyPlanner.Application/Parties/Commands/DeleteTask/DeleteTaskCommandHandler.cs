using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.DeleteTask;

public sealed class DeleteTaskCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<DeleteTaskCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);
        if (party.Tasks.All(t => t.Id != request.TaskId)) return null;

        await partyRepository.DeleteTaskAsync(party.Id, request.TaskId, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new TaskDeletedEvent(request.OwnerUserId, party.Name), cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }
}
