using PartyPlanner.Core.DTO.Responses;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Core.Extensions;

public static class PartyMappingExtensions
{
    public static PartyResponse ToResponse(this Party party)
    {
        return new PartyResponse(
            party.Id,
            party.Name,
            party.Category,
            party.Date,
            party.Location,
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
}
