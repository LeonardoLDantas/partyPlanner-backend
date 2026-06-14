using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.UpdateTask;

public sealed class UpdateTaskCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<UpdateTaskCommand, PartyResponse?>
{
    public async Task<PartyResponse?> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return null;

        party.EnsureAcceptingChangesOn(dateTimeProvider.Today);
        var currentTask = party.Tasks.FirstOrDefault(t => t.Id == request.TaskId);
        if (currentTask is null) return null;

        var status = NormalizeTaskStatus(request.Status ?? currentTask.Status);
        var title = string.IsNullOrWhiteSpace(request.Title) ? currentTask.Title : request.Title.Trim();
        var assignee = string.IsNullOrWhiteSpace(request.Assignee) ? currentTask.Assignee : request.Assignee.Trim();
        var description = request.Description is null ? currentTask.Description : request.Description.Trim();

        if (!party.UpdateTask(request.TaskId, title, assignee, description, status)) return null;

        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new TaskUpdatedEvent(request.OwnerUserId, status, party.Name), cancellationToken);

        return party.ToResponse();
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
