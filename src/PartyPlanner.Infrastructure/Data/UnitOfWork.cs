using PartyPlanner.Core.Interfaces;

namespace PartyPlanner.Infrastructure.Data;

public sealed class UnitOfWork(PartyPlannerDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
        dbContext.ChangeTracker.Clear();
    }
}
