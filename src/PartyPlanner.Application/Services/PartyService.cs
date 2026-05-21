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
        var currentDate = GetCurrentBusinessDate();
        if (parties.Any(party => party.FinalizeIfPast(currentDate)))
        {
            await partyRepository.SaveChangesAsync(cancellationToken);
        }

        return parties.Select(party => party.ToResponse()).ToArray();
    }

    public async Task<PartyResponse?> GetByIdAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(id, ownerUserId, cancellationToken);
        if (party?.FinalizeIfPast(GetCurrentBusinessDate()) == true)
        {
            await partyRepository.SaveChangesAsync(cancellationToken);
        }

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
            string.IsNullOrWhiteSpace(request.Time) ? "19:00" : request.Time.Trim(),
            string.IsNullOrWhiteSpace(request.Location) ? "Local a definir" : request.Location.Trim(),
            string.IsNullOrWhiteSpace(request.CoverImageUrl) ? string.Empty : request.CoverImageUrl.Trim(),
            Math.Max(request.ExpectedGuests ?? 0, 0),
            new Budget(request.EstimatedBudget, 0, []),
            request.IsFinalized ?? false
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
            string.IsNullOrWhiteSpace(request.Time) ? "19:00" : request.Time.Trim(),
            string.IsNullOrWhiteSpace(request.Location) ? "Local a definir" : request.Location.Trim(),
            string.IsNullOrWhiteSpace(request.CoverImageUrl) ? party.CoverImageUrl : request.CoverImageUrl.Trim(),
            Math.Max(request.ExpectedGuests ?? party.ExpectedGuests, 0),
            request.EstimatedBudget,
            request.IsFinalized ?? party.IsFinalized);

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

        party.EnsureAcceptingChangesOn(GetCurrentBusinessDate());

        var task = new PartyTask(
            Guid.NewGuid(),
            request.Title.Trim(),
            string.IsNullOrWhiteSpace(request.Assignee) ? "Sem responsável" : request.Assignee.Trim(),
            string.IsNullOrWhiteSpace(request.DueDate) ? party.Date : request.DueDate.Trim(),
            string.IsNullOrWhiteSpace(request.Description) ? string.Empty : request.Description.Trim(),
            NormalizeTaskStatus(request.Status),
            false);

        await partyRepository.AddTaskAsync(party.Id, task, cancellationToken);
        await partyRepository.SaveChangesAsync(cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Tarefa adicionada",
            $"A tarefa \"{request.Title.Trim()}\" foi adicionada em \"{party.Name}\".",
            "task",
            cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }

    public async Task<PartyResponse?> ToggleTaskAsync(Guid ownerUserId, Guid partyId, Guid taskId, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureAcceptingChangesOn(GetCurrentBusinessDate());
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

    public async Task<PartyResponse?> UpdateTaskStatusAsync(Guid ownerUserId, Guid partyId, Guid taskId, UpdateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureAcceptingChangesOn(GetCurrentBusinessDate());
        var currentTask = party.Tasks.FirstOrDefault(task => task.Id == taskId);
        if (currentTask is null)
        {
            return null;
        }

        var status = NormalizeTaskStatus(request.Status ?? currentTask.Status);
        var title = string.IsNullOrWhiteSpace(request.Title) ? currentTask.Title : request.Title.Trim();
        var assignee = string.IsNullOrWhiteSpace(request.Assignee) ? currentTask.Assignee : request.Assignee.Trim();
        var description = request.Description is null ? currentTask.Description : request.Description.Trim();
        if (!party.UpdateTask(taskId, title, assignee, description, status))
        {
            return null;
        }

        await partyRepository.SaveChangesAsync(cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Tarefa movida",
            $"Uma tarefa foi movida para \"{status}\" em \"{party.Name}\".",
            "task",
            cancellationToken);
        return party.ToResponse();
    }

    public async Task<PartyResponse?> DeleteTaskAsync(Guid ownerUserId, Guid partyId, Guid taskId, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureAcceptingChangesOn(GetCurrentBusinessDate());
        if (party.Tasks.All(task => task.Id != taskId))
        {
            return null;
        }

        await partyRepository.DeleteTaskAsync(party.Id, taskId, cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Tarefa removida",
            $"Uma tarefa foi removida de \"{party.Name}\".",
            "task",
            cancellationToken);
        await partyRepository.SaveChangesAsync(cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }

    public async Task<PartyResponse?> AddGuestAsync(Guid ownerUserId, Guid partyId, CreateGuestRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureAcceptingChangesOn(GetCurrentBusinessDate());

        var guest = new Guest(
            Guid.NewGuid(),
            request.Name.Trim(),
            string.IsNullOrWhiteSpace(request.Group) ? "Geral" : request.Group.Trim(),
            request.Type ?? GuestType.Adulto,
            "Pendente",
            CreateInvitationToken(),
            string.IsNullOrWhiteSpace(request.Email) ? string.Empty : request.Email.Trim(),
            string.IsNullOrWhiteSpace(request.PhoneNumber) ? string.Empty : request.PhoneNumber.Trim()
        );

        await partyRepository.AddGuestAsync(party.Id, guest, cancellationToken);
        await partyRepository.SaveChangesAsync(cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Convidado adicionado",
            $"\"{request.Name.Trim()}\" foi adicionado a lista de convidados.",
            "guest",
            cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }

    public async Task<PartyResponse?> DeleteGuestAsync(Guid ownerUserId, Guid partyId, Guid guestId, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureAcceptingChangesOn(GetCurrentBusinessDate());
        if (party.Guests.All(guest => guest.Id != guestId))
        {
            return null;
        }

        await partyRepository.DeleteGuestAsync(party.Id, guestId, cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Convidado removido",
            $"Um convidado foi removido de \"{party.Name}\".",
            "guest",
            cancellationToken);
        await partyRepository.SaveChangesAsync(cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }

    public async Task<InvitationResponse?> GetInvitationAsync(string token, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByInvitationTokenAsync(token, cancellationToken);
        var guest = party?.Guests.FirstOrDefault(currentGuest => currentGuest.InvitationToken == token);
        return party is null || guest is null ? null : ToInvitationResponse(party, guest);
    }

    public async Task<InvitationResponse?> RespondInvitationAsync(string token, RespondInvitationRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByInvitationTokenAsync(token, cancellationToken);
        var guest = party?.Guests.FirstOrDefault(currentGuest => currentGuest.InvitationToken == token);
        if (party is null || guest is null)
        {
            return null;
        }

        var status = NormalizeInvitationStatus(request.Status);
        guest.UpdateStatus(status);
        await partyRepository.SaveChangesAsync(cancellationToken);
        await notificationService.CreateAsync(
            party.OwnerUserId,
            "Resposta de convite",
            $"\"{guest.Name}\" marcou presenca como {status} em \"{party.Name}\".",
            "guest",
            cancellationToken);

        return ToInvitationResponse(party, guest);
    }

    public async Task<PartyResponse?> AddBudgetItemAsync(Guid ownerUserId, Guid partyId, CreateBudgetItemRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureAcceptingChangesOn(GetCurrentBusinessDate());

        var budgetItem = new BudgetItem(
            Guid.NewGuid(),
            request.Label.Trim(),
            request.Category ?? ExpenseCategory.Outros,
            request.Amount,
            request.IsPaid);

        await partyRepository.AddBudgetItemAsync(party.Id, budgetItem, cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Despesa adicionada",
            $"A despesa \"{request.Label.Trim()}\" foi registrada no evento \"{party.Name}\".",
            "budget",
            cancellationToken);
        await partyRepository.SaveChangesAsync(cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }

    public async Task<PartyResponse?> UpdateBudgetItemAsync(Guid ownerUserId, Guid partyId, Guid budgetItemId, CreateBudgetItemRequest request, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureAcceptingChangesOn(GetCurrentBusinessDate());
        await partyRepository.UpdateBudgetItemAsync(party.Id, budgetItemId, request.Amount, request.IsPaid, cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Despesa atualizada",
            $"Uma despesa do evento \"{party.Name}\" foi atualizada.",
            "budget",
            cancellationToken);
        await partyRepository.SaveChangesAsync(cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
    }

    public async Task<PartyResponse?> DeleteBudgetItemAsync(Guid ownerUserId, Guid partyId, Guid budgetItemId, CancellationToken cancellationToken = default)
    {
        var party = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        if (party is null)
        {
            return null;
        }

        party.EnsureAcceptingChangesOn(GetCurrentBusinessDate());
        await partyRepository.DeleteBudgetItemAsync(party.Id, budgetItemId, cancellationToken);
        await notificationService.CreateAsync(
            ownerUserId,
            "Despesa removida",
            $"Uma despesa do evento \"{party.Name}\" foi removida.",
            "budget",
            cancellationToken);
        await partyRepository.SaveChangesAsync(cancellationToken);

        var updatedParty = await partyRepository.GetByIdAsync(partyId, ownerUserId, cancellationToken);
        return (updatedParty ?? party).ToResponse();
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

    private static string CreateInvitationToken()
    {
        return Convert.ToHexString(Guid.NewGuid().ToByteArray()).ToLowerInvariant();
    }

    private static string NormalizeInvitationStatus(string status)
    {
        return status.Trim().Equals("Recusou", StringComparison.OrdinalIgnoreCase) ? "Recusou" : "Confirmado";
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

    private static InvitationResponse ToInvitationResponse(Party party, Guest guest)
    {
        return new InvitationResponse(
            guest.InvitationToken,
            guest.Name,
            guest.Status,
            party.Name,
            party.Date,
            party.Time,
            party.Location,
            party.CoverImageUrl);
    }
}
