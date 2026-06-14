namespace PartyPlanner.Application.Interfaces;

public interface INotificationService
{
    Task CreateAsync(Guid userId, string title, string message, string type, CancellationToken cancellationToken = default);
}
