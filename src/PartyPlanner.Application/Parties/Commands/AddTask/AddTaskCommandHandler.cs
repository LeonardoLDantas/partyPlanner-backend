using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.AddTask;

public sealed class AddTaskCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<AddTaskCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(AddTaskCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);

        var task = new EntityPartyTask(
            Guid.NewGuid(),
            request.Title.Trim(),
            string.IsNullOrWhiteSpace(request.Assignee) ? "Sem responsável" : request.Assignee.Trim(),
            string.IsNullOrWhiteSpace(request.DueDate) ? party.Date : request.DueDate.Trim(),
            string.IsNullOrWhiteSpace(request.Description) ? string.Empty : request.Description.Trim(),
            NormalizeTaskStatus(request.Status),
            false);

        await partyRepository.AddTaskAsync(party.Id, task, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new TaskAddedEvent(request.OwnerUserId, request.Title.Trim(), party.Name), cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }

    private static string NormalizeTaskStatus(string? status)
    {
        var normalized = string.IsNullOrWhiteSpace(status) ? "Pendente" : status.Trim();
        return normalized.ToLowerInvariant() switch
        {
            "em andamento" => "Em andamento",
            "concluida" or "concluída" or "feito" or "feita" => "Concluída",
            _ => "Pendente"
        };
    }
}
