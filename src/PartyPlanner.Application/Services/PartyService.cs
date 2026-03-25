using PartyPlanner.Application.Interface;
using PartyPlanner.Core.DTO.Requests;
using PartyPlanner.Core.DTO.Responses;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Extensions;

namespace PartyPlanner.Application.Services;

public sealed class PartyService(IPartyRepository partyRepository) : IPartyService
{
    public async Task<IReadOnlyCollection<PartyResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var parties = await partyRepository.GetAllAsync(cancellationToken);
        return parties.Select(party => party.ToResponse()).ToArray();
    }

    public async Task<PartyResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(id, cancellationToken);
        return party?.ToResponse();
    }

    public async Task<PartyResponse> CreateAsync(CreatePartyRequest request, CancellationToken cancellationToken = default)
    {
        var party = new Party(
            Guid.NewGuid(),
            request.Name.Trim(),
            string.IsNullOrWhiteSpace(request.Category) ? "Evento" : request.Category.Trim(),
            string.IsNullOrWhiteSpace(request.Date) ? "Data a definir" : request.Date.Trim(),
            string.IsNullOrWhiteSpace(request.Location) ? "Local a definir" : request.Location.Trim(),
            new Budget(request.EstimatedBudget, 0, [])
        );

        await partyRepository.AddAsync(party, cancellationToken);
        await partyRepository.SaveChangesAsync(cancellationToken);
        return party.ToResponse();
    }

    public async Task<PartyResponse?> AddTaskAsync(Guid partyId, CreateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.AddTask(new PartyTask(
            Guid.NewGuid(),
            request.Title.Trim(),
            string.IsNullOrWhiteSpace(request.Assignee) ? "Sem responsavel" : request.Assignee.Trim(),
            false
        ));

        await partyRepository.SaveChangesAsync(cancellationToken);
        return party.ToResponse();
    }

    public async Task<PartyResponse?> ToggleTaskAsync(Guid partyId, Guid taskId, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, cancellationToken);
        if (party is null || !party.ToggleTask(taskId))
        {
            return null;
        }

        await partyRepository.SaveChangesAsync(cancellationToken);
        return party.ToResponse();
    }

    public async Task<PartyResponse?> AddGuestAsync(Guid partyId, CreateGuestRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.AddGuest(new Guest(
            Guid.NewGuid(),
            request.Name.Trim(),
            string.IsNullOrWhiteSpace(request.Group) ? "Geral" : request.Group.Trim(),
            string.IsNullOrWhiteSpace(request.Status) ? "Pendente" : request.Status.Trim()
        ));

        await partyRepository.SaveChangesAsync(cancellationToken);
        return party.ToResponse();
    }

    public async Task<PartyResponse?> AddBudgetItemAsync(Guid partyId, CreateBudgetItemRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.AddBudgetItem(new BudgetItem(
            Guid.NewGuid(),
            request.Label.Trim(),
            string.IsNullOrWhiteSpace(request.Category) ? "Geral" : request.Category.Trim(),
            request.Amount
        ));

        await partyRepository.SaveChangesAsync(cancellationToken);
        return party.ToResponse();
    }
}
