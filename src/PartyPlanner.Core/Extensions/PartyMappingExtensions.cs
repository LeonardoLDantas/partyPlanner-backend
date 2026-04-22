using PartyPlanner.Core.DTO.Responses;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Core.Extensions;

public static class PartyMappingExtensions
{
    private static readonly string[] BusinessTimeZoneIds = ["America/Sao_Paulo", "E. South America Standard Time"];

    public static PartyResponse ToResponse(this Party party)
    {
        return new PartyResponse(
            party.Id,
            party.OwnerUserId,
            party.Name,
            party.Category,
            party.Date,
            party.Location,
            party.CanBeEditedOn(GetCurrentBusinessDate()),
            party.Tasks
                .Select(task => new PartyTaskResponse(task.Id, task.Title, task.Assignee, task.Done))
                .ToArray(),
            party.Guests
                .Select(guest => new GuestResponse(guest.Id, guest.Name, guest.Group, guest.Status))
                .ToArray(),
            new BudgetResponse(
                party.Budget.Estimated,
                party.Budget.Spent,
                party.Budget.Items
                    .Select(item => new BudgetItemResponse(item.Id, item.Label, item.Category, item.Amount))
                    .ToArray()
            )
        );
    }

    private static DateOnly GetCurrentBusinessDate()
    {
        foreach (var timeZoneId in BusinessTimeZoneIds)
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone));
            }
            catch (TimeZoneNotFoundException)
            {
            }
            catch (InvalidTimeZoneException)
            {
            }
        }

        return DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
