using PartyPlanner.Core.Entities;

namespace PartyPlanner.Application.Interface;

public interface IPartyRepository
{
    Task<IReadOnlyCollection<Party>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Party?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Party party, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
