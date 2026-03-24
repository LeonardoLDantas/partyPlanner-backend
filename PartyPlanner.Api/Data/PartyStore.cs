using PartyPlanner.Api.Models;

namespace PartyPlanner.Api.Data;

public sealed class PartyStore
{
    private readonly List<Party> _parties =
    [
        new Party
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Aniversario da Sofia",
            Category = "Aniversario",
            Date = "12 de abril de 2026",
            Location = "Espaco Jardim Azul",
            Tasks =
            [
                new PartyTask
                {
                    Id = Guid.Parse("21111111-1111-1111-1111-111111111111"),
                    Title = "Fechar buffet infantil",
                    Assignee = "Luiza",
                    Done = true
                },
                new PartyTask
                {
                    Id = Guid.Parse("21111111-1111-1111-1111-111111111112"),
                    Title = "Confirmar decoracao tema sereia",
                    Assignee = "Marina",
                    Done = false
                }
            ],
            Guests =
            [
                new Guest
                {
                    Id = Guid.Parse("31111111-1111-1111-1111-111111111111"),
                    Name = "Ana e familia",
                    Group = "Familia",
                    Status = "Confirmado"
                },
                new Guest
                {
                    Id = Guid.Parse("31111111-1111-1111-1111-111111111112"),
                    Name = "Escola Arco-Iris",
                    Group = "Amigos da escola",
                    Status = "Pendente"
                }
            ],
            Budget = new Budget
            {
                Estimated = 8500,
                Spent = 4250,
                Items =
                [
                    new BudgetItem
                    {
                        Id = Guid.Parse("41111111-1111-1111-1111-111111111111"),
                        Label = "Buffet",
                        Category = "Alimentacao",
                        Amount = 2800
                    },
                    new BudgetItem
                    {
                        Id = Guid.Parse("41111111-1111-1111-1111-111111111112"),
                        Label = "Decoracao",
                        Category = "Ambiente",
                        Amount = 1450
                    }
                ]
            }
        }
    ];

    public IReadOnlyList<Party> GetAll() => _parties;

    public Party? GetById(Guid id) => _parties.FirstOrDefault(party => party.Id == id);

    public void AddParty(Party party) => _parties.Insert(0, party);

    public Party? AddTask(Guid partyId, PartyTask task)
    {
        var party = GetById(partyId);
        if (party is null)
        {
            return null;
        }

        party.Tasks.Insert(0, task);
        return party;
    }

    public Party? AddGuest(Guid partyId, Guest guest)
    {
        var party = GetById(partyId);
        if (party is null)
        {
            return null;
        }

        party.Guests.Insert(0, guest);
        return party;
    }

    public Party? AddBudgetItem(Guid partyId, BudgetItem item)
    {
        var party = GetById(partyId);
        if (party is null)
        {
            return null;
        }

        party.Budget.Items.Insert(0, item);
        party.Budget.Spent += item.Amount;
        return party;
    }
}
