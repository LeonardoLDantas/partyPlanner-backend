using Microsoft.EntityFrameworkCore;
using PartyPlanner.Application.Interface;
using PartyPlanner.Core.Entities;
using PartyPlanner.Infrastructure.Data;

namespace PartyPlanner.Infrastructure.Repository;

public sealed class PartyRepository(PartyPlannerDbContext dbContext) : IPartyRepository
{
    public async Task<IReadOnlyCollection<Party>> GetAllAsync(Guid ownerUserId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Parties
            .Where(party => party.OwnerUserId == ownerUserId)
            .Include(party => party.Tasks)
            .Include(party => party.Guests)
            .Include(party => party.Budget.Items)
            .ToListAsync(cancellationToken);
    }

    public async Task<Party?> GetByIdAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Parties
            .Where(party => party.OwnerUserId == ownerUserId)
            .Include(party => party.Tasks)
            .Include(party => party.Guests)
            .Include(party => party.Budget.Items)
            .FirstOrDefaultAsync(party => party.Id == id, cancellationToken);
    }

    public async Task<Party?> GetByInvitationTokenAsync(string invitationToken, CancellationToken cancellationToken = default)
    {
        return await dbContext.Parties
            .Include(party => party.Tasks)
            .Include(party => party.Guests)
            .Include(party => party.Budget.Items)
            .FirstOrDefaultAsync(party => party.Guests.Any(guest => guest.InvitationToken == invitationToken), cancellationToken);
    }

    public async Task AddAsync(Party party, CancellationToken cancellationToken = default)
    {
        await dbContext.Parties.AddAsync(party, cancellationToken);
    }

    public async Task AddGuestAsync(Guid partyId, Guest guest, CancellationToken cancellationToken = default)
    {
        await dbContext.Guests.AddAsync(guest, cancellationToken);
        dbContext.Entry(guest).Property<Guid?>("PartyId").CurrentValue = partyId;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
