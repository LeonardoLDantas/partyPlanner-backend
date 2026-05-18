using PartyPlanner.Core.Entities;

namespace PartyPlanner.Application.Interface;

public interface IPartyRepository
{
    Task<IReadOnlyCollection<Party>> GetAllAsync(Guid ownerUserId, CancellationToken cancellationToken = default);
    Task<Party?> GetByIdAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken = default);
    Task<Party?> GetByInvitationTokenAsync(string invitationToken, CancellationToken cancellationToken = default);
    Task AddAsync(Party party, CancellationToken cancellationToken = default);
    Task AddGuestAsync(Guid partyId, Guest guest, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
