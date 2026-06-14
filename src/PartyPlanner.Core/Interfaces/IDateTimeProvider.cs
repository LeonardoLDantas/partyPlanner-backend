namespace PartyPlanner.Core.Interfaces;

public interface IDateTimeProvider
{
    DateOnly Today { get; }
    DateTime Now { get; }
}
