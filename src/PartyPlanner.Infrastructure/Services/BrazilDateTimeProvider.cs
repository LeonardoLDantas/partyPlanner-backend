using PartyPlanner.Core.Interfaces;

namespace PartyPlanner.Infrastructure.Services;

public sealed class BrazilDateTimeProvider : IDateTimeProvider
{
    private static readonly string[] TimeZoneIds = ["America/Sao_Paulo", "E. South America Standard Time"];

    public DateOnly Today => DateOnly.FromDateTime(Now);

    public DateTime Now
    {
        get
        {
            foreach (var id in TimeZoneIds)
            {
                try
                {
                    var tz = TimeZoneInfo.FindSystemTimeZoneById(id);
                    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
                }
                catch (TimeZoneNotFoundException) { }
                catch (InvalidTimeZoneException) { }
            }
            return DateTime.UtcNow;
        }
    }
}
