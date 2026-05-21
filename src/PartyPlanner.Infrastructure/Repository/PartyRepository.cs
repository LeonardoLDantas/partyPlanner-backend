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

    public async Task<bool> DeleteAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken = default)
    {
        var affectedRows = await dbContext.Parties
            .Where(party => party.Id == id && party.OwnerUserId == ownerUserId)
            .ExecuteDeleteAsync(cancellationToken);

        dbContext.ChangeTracker.Clear();
        return affectedRows > 0;
    }

    public async Task AddTaskAsync(Guid partyId, PartyTask task, CancellationToken cancellationToken = default)
    {
        await dbContext.Tasks.AddAsync(task, cancellationToken);
        dbContext.Entry(task).Property<Guid?>("PartyId").CurrentValue = partyId;
    }

    public async Task DeleteTaskAsync(Guid partyId, Guid taskId, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             DELETE FROM "Tasks"
             WHERE "Id" = {taskId} AND "PartyId" = {partyId}
             """,
            cancellationToken);

        dbContext.ChangeTracker.Clear();
    }

    public async Task AddGuestAsync(Guid partyId, Guest guest, CancellationToken cancellationToken = default)
    {
        await dbContext.Guests.AddAsync(guest, cancellationToken);
        dbContext.Entry(guest).Property<Guid?>("PartyId").CurrentValue = partyId;
    }

    public async Task DeleteGuestAsync(Guid partyId, Guid guestId, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             DELETE FROM "Guests"
             WHERE "Id" = {guestId} AND "PartyId" = {partyId}
             """,
            cancellationToken);

        dbContext.ChangeTracker.Clear();
    }

    public async Task AddBudgetItemAsync(Guid partyId, BudgetItem item, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             INSERT INTO "BudgetItems" ("Id", "PartyId", "Label", "Category", "Amount", "IsPaid")
             VALUES ({item.Id}, {partyId}, {item.Label}, {item.Category.ToString()}, {item.Amount}, {item.IsPaid})
             """,
            cancellationToken);

        await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             UPDATE "Parties"
             SET "BudgetSpent" = "BudgetSpent" + {item.Amount}
             WHERE "Id" = {partyId}
             """,
            cancellationToken);

        dbContext.ChangeTracker.Clear();
    }

    public async Task UpdateBudgetItemAsync(Guid partyId, Guid budgetItemId, decimal amount, bool isPaid, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             UPDATE "Parties"
             SET "BudgetSpent" = "BudgetSpent" + {amount} - COALESCE((
                 SELECT "Amount" FROM "BudgetItems"
                 WHERE "Id" = {budgetItemId} AND "PartyId" = {partyId}
             ), 0)
             WHERE "Id" = {partyId}
             """,
            cancellationToken);

        await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             UPDATE "BudgetItems"
             SET "Amount" = {amount}, "IsPaid" = {isPaid}
             WHERE "Id" = {budgetItemId} AND "PartyId" = {partyId}
             """,
            cancellationToken);

        dbContext.ChangeTracker.Clear();
    }

    public async Task DeleteBudgetItemAsync(Guid partyId, Guid budgetItemId, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             UPDATE "Parties"
             SET "BudgetSpent" = GREATEST("BudgetSpent" - COALESCE((
                 SELECT "Amount" FROM "BudgetItems"
                 WHERE "Id" = {budgetItemId} AND "PartyId" = {partyId}
             ), 0), 0)
             WHERE "Id" = {partyId}
             """,
            cancellationToken);

        await dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             DELETE FROM "BudgetItems"
             WHERE "Id" = {budgetItemId} AND "PartyId" = {partyId}
             """,
            cancellationToken);

        dbContext.ChangeTracker.Clear();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
