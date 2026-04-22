using PartyPlanner.Application.Interface;
using PartyPlanner.Core.DTO.Requests;
using PartyPlanner.Core.DTO.Responses;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Enums;
using PartyPlanner.Core.Extensions;

namespace PartyPlanner.Application.Services;

public sealed class PartyService(
    IPartyRepository partyRepository,
    INotificationService notificationService) : IPartyService
{
    private static readonly string[] BusinessTimeZoneIds = ["America/Sao_Paulo", "E. South America Standard Time"];

    public async Task<IReadOnlyCollection<PartyResponse>> GetAllAsync(Guid ownerUserId, CancellationToken cancellationToken = default)
    {
        var parties = await partyRepository.GetAllAsync(ownerUserId, cancellationToken);
        return parties.Select(party => party.ToResponse()).ToArray();
    }

    public async Task<PartyResponse?> GetByIdAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(id, ownerUserId, cancellationToken);
        return party?.ToResponse();
    }

    public async Task<PartyResponse> CreateAsync(Guid ownerUserId, CreatePartyRequest request, CancellationToken cancellationToken = default)
    {
        var party = new Party(
            Guid.NewGuid(),
            ownerUserId,
            request.Name.Trim(),
            request.Category ?? PartyCategory.Outros,
            string.IsNullOrWhiteSpace(request.Date) ? "Data a definir" : request.Date.Trim(),
            string.IsNullOrWhiteSpace(request.Location) ? "Local a definir" : request.Location.Trim(),
            new Budget(request.EstimatedBudget, 0, [])
        );

        await partyRepository.AddAsync(party, cancellationToken);
        await partyRepository.SaveChangesAsync(cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Nova festa criada",
            $"A festa \"{party.Name}\" foi criada com sucesso.",
            "party",
            cancellationToken);
        return party.ToResponse();
    }

    public async Task<PartyResponse?> UpdateAsync(Guid ownerUserId, Guid partyId, UpdatePartyRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureEditableOn(GetCurrentBusinessDate());
        party.UpdateDetails(
            request.Name.Trim(),
            request.Category ?? PartyCategory.Outros,
            string.IsNullOrWhiteSpace(request.Date) ? "Data a definir" : request.Date.Trim(),
            string.IsNullOrWhiteSpace(request.Location) ? "Local a definir" : request.Location.Trim(),
            request.EstimatedBudget);

        await partyRepository.SaveChangesAsync(cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Festa atualizada",
            $"A festa \"{party.Name}\" foi atualizada pelo usuario criador.",
            "party",
            cancellationToken);
        return party.ToResponse();
    }

    public async Task<PartyResponse?> AddTaskAsync(Guid ownerUserId, Guid partyId, CreateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureEditableOn(GetCurrentBusinessDate());

        party.AddTask(new PartyTask(
            Guid.NewGuid(),
            request.Title.Trim(),
            string.IsNullOrWhiteSpace(request.Assignee) ? "Sem responsavel" : request.Assignee.Trim(),
            false
        ));

        await partyRepository.SaveChangesAsync(cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Tarefa adicionada",
            $"A tarefa \"{request.Title.Trim()}\" foi adicionada em \"{party.Name}\".",
            "task",
            cancellationToken);
        return party.ToResponse();
    }

    public async Task<PartyResponse?> ToggleTaskAsync(Guid ownerUserId, Guid partyId, Guid taskId, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureEditableOn(GetCurrentBusinessDate());
        if (!party.ToggleTask(taskId))
        {
            return null;
        }

        await partyRepository.SaveChangesAsync(cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Tarefa atualizada",
            "O status de uma tarefa foi atualizado.",
            "task",
            cancellationToken);
        return party.ToResponse();
    }

    public async Task<PartyResponse?> AddGuestAsync(Guid ownerUserId, Guid partyId, CreateGuestRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureEditableOn(GetCurrentBusinessDate());

        party.AddGuest(new Guest(
            Guid.NewGuid(),
            request.Name.Trim(),
            string.IsNullOrWhiteSpace(request.Group) ? "Geral" : request.Group.Trim(),
            string.IsNullOrWhiteSpace(request.Status) ? "Pendente" : request.Status.Trim()
        ));

        await partyRepository.SaveChangesAsync(cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Convidado adicionado",
            $"\"{request.Name.Trim()}\" foi adicionado a lista de convidados.",
            "guest",
            cancellationToken);
        return party.ToResponse();
    }

    public async Task<PartyResponse?> AddBudgetItemAsync(Guid ownerUserId, Guid partyId, CreateBudgetItemRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureEditableOn(GetCurrentBusinessDate());

        party.AddBudgetItem(new BudgetItem(
            Guid.NewGuid(),
            request.Label.Trim(),
            string.IsNullOrWhiteSpace(request.Category) ? "Geral" : request.Category.Trim(),
            request.Amount
        ));

        await partyRepository.SaveChangesAsync(cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Despesa adicionada",
            $"A despesa \"{request.Label.Trim()}\" foi registrada no evento \"{party.Name}\".",
            "budget",
            cancellationToken);
        return party.ToResponse();
    }

    private static DateOnly GetCurrentBusinessDate()
    {
        foreach (var timeZoneId in BusinessTimeZoneIds)
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone));
            }
            catch (TimeZoneNotFoundException)
            {
            }
            catch (InvalidTimeZoneException)
            {
            }
        }

        return DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
