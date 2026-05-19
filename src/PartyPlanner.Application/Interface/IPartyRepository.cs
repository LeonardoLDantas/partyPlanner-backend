using PartyPlanner.Core.Entities;

namespace PartyPlanner.Application.Interface;

public interface IPartyRepository
{
    Task<IReadOnlyCollection<Party>> GetAllAsync(Guid ownerUserId, CancellationToken cancellationToken = default);
    Task<Party?> GetByIdAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken = default);
    Task<Party?> GetByInvitationTokenAsync(string invitationToken, CancellationToken cancellationToken = default);
    Task AddAsync(Party party, CancellationToken cancellationToken = default);
    Task AddTaskAsync(Guid partyId, PartyTask task, CancellationToken cancellationToken = default);
    Task DeleteTaskAsync(Guid partyId, Guid taskId, CancellationToken cancellationToken = default);
    Task AddGuestAsync(Guid partyId, Guest guest, CancellationToken cancellationToken = default);
    Task DeleteGuestAsync(Guid partyId, Guid guestId, CancellationToken cancellationToken = default);
    Task AddBudgetItemAsync(Guid partyId, BudgetItem item, CancellationToken cancellationToken = default);
    Task UpdateBudgetItemAsync(Guid partyId, Guid budgetItemId, decimal amount, CancellationToken cancellationToken = default);
    Task DeleteBudgetItemAsync(Guid partyId, Guid budgetItemId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
