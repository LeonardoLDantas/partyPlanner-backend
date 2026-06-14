using PartyPlanner.Core.Entities;

namespace PartyPlanner.Core.Interfaces.Repositories;

public interface IPartyRepository
{
    Task<IReadOnlyCollection<EntityParty>> GetAllAsync(Guid ownerUserId, CancellationToken cancellationToken = default);
    Task<EntityParty?> GetByIdAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken = default);
    Task<EntityParty?> GetByInvitationTokenAsync(string invitationToken, CancellationToken cancellationToken = default);
    Task AddAsync(EntityParty party, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken = default);
    Task AddTaskAsync(Guid partyId, EntityPartyTask task, CancellationToken cancellationToken = default);
    Task DeleteTaskAsync(Guid partyId, Guid taskId, CancellationToken cancellationToken = default);
    Task AddGuestAsync(Guid partyId, EntityGuest guest, CancellationToken cancellationToken = default);
    Task DeleteGuestAsync(Guid partyId, Guid guestId, CancellationToken cancellationToken = default);
    Task AddBudgetItemAsync(Guid partyId, EntityBudgetItem item, CancellationToken cancellationToken = default);
    Task UpdateBudgetItemAsync(Guid partyId, Guid budgetItemId, decimal amount, bool isPaid, CancellationToken cancellationToken = default);
    Task DeleteBudgetItemAsync(Guid partyId, Guid budgetItemId, CancellationToken cancellationToken = default);
}
