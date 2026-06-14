using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Application.Mappings;

public static class PartyMappingExtensions
{
    private static readonly string[] BusinessTimeZoneIds = ["America/Sao_Paulo", "E. South America Standard Time"];

    public static PartyResponse ToResponse(this EntityParty party)
    {
        var currentDate = GetCurrentBusinessDate();

        return new PartyResponse(
            party.Id,
            party.OwnerUserId,
            party.Name,
            party.Category,
            party.Date,
            party.Time,
            party.Location,
            party.CoverImageUrl,
            party.ExpectedGuests,
            party.IsFinalizedOn(currentDate),
            party.CanBeEditedOn(currentDate),
            party.Tasks
                .Select(task => new PartyTaskResponse(task.Id, task.Title, task.Assignee, task.DueDate, task.Description, task.Status, task.Done))
                .ToArray(),
            party.Guests
                .Select(guest => new GuestResponse(guest.Id, guest.Name, guest.Group, guest.Type, guest.Status, guest.InvitationToken, guest.Email, guest.PhoneNumber))
                .ToArray(),
            new BudgetResponse(
                party.EntityBudget.Estimated,
                party.EntityBudget.Spent,
                party.EntityBudget.Items
                    .Select(item => new BudgetItemResponse(item.Id, item.Label, item.Category, item.Amount, item.IsPaid))
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
