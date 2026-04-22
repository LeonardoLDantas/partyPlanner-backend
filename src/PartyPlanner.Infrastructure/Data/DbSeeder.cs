using Microsoft.EntityFrameworkCore;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Enums;
using PartyPlanner.Infrastructure.Security;

namespace PartyPlanner.Infrastructure.Data;

public sealed class DbSeeder(PartyPlannerDbContext dbContext)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        var demoUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var demoUser = await dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == demoUserId, cancellationToken);

        if (demoUser is null)
        {
            var passwordHasher = new Pbkdf2PasswordHasher();
            demoUser = new User(
                demoUserId,
                "Demo Party Planner",
                "demo@partyplanner.app",
                passwordHasher.Hash("Party123!"),
                true
            );

            await dbContext.Users.AddAsync(demoUser, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        if (await dbContext.Parties.AnyAsync(cancellationToken))
        {
            return;
        }

        var party = new Party(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            demoUser.Id,
            "Aniversario da Sofia",
            PartyCategory.Aniversario,
            "12 de abril de 2026",
            "Espaco Jardim Azul",
            new Budget(
                8500,
                4250,
                [
                    new BudgetItem(
                        Guid.Parse("41111111-1111-1111-1111-111111111111"),
                        "Buffet",
                        "Alimentacao",
                        2800
                    ),
                    new BudgetItem(
                        Guid.Parse("41111111-1111-1111-1111-111111111112"),
                        "Decoracao",
                        "Ambiente",
                        1450
                    )
                ]
            )
        );

        party.AddTask(new PartyTask(
            Guid.Parse("21111111-1111-1111-1111-111111111111"),
            "Fechar buffet infantil",
            "Luiza",
            true
        ));

        party.AddTask(new PartyTask(
            Guid.Parse("21111111-1111-1111-1111-111111111112"),
            "Confirmar decoracao tema sereia",
            "Marina",
            false
        ));

        party.AddGuest(new Guest(
            Guid.Parse("31111111-1111-1111-1111-111111111111"),
            "Ana e familia",
            "Familia",
            "Confirmado"
        ));

        party.AddGuest(new Guest(
            Guid.Parse("31111111-1111-1111-1111-111111111112"),
            "Escola Arco-Iris",
            "Amigos da escola",
            "Pendente"
        ));

        await dbContext.Parties.AddAsync(party, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
