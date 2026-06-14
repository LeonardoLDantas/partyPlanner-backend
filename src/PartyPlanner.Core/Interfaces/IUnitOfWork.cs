namespace PartyPlanner.Core.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
