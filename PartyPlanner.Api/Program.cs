using PartyPlanner.Api.Contracts;
using PartyPlanner.Api.Data;
using PartyPlanner.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<PartyStore>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "PartyPlanner.Api" }));

app.MapGet("/api/parties", (PartyStore store) => Results.Ok(store.GetAll()));

app.MapGet("/api/parties/{id:guid}", (Guid id, PartyStore store) =>
{
    var party = store.GetById(id);
    return party is null ? Results.NotFound() : Results.Ok(party);
});

app.MapPost("/api/parties", (CreatePartyRequest request, PartyStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["name"] = ["Name is required."]
        });
    }

    var party = new Party
    {
        Id = Guid.NewGuid(),
        Name = request.Name.Trim(),
        Category = string.IsNullOrWhiteSpace(request.Category) ? "Evento" : request.Category.Trim(),
        Date = string.IsNullOrWhiteSpace(request.Date) ? "Data a definir" : request.Date.Trim(),
        Location = string.IsNullOrWhiteSpace(request.Location) ? "Local a definir" : request.Location.Trim(),
        Budget = new Budget
        {
            Estimated = request.EstimatedBudget,
            Spent = 0
        }
    };

    store.AddParty(party);
    return Results.Created($"/api/parties/{party.Id}", party);
});

app.MapPost("/api/parties/{partyId:guid}/tasks", (Guid partyId, CreateTaskRequest request, PartyStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["title"] = ["Title is required."]
        });
    }

    var task = new PartyTask
    {
        Id = Guid.NewGuid(),
        Title = request.Title.Trim(),
        Assignee = string.IsNullOrWhiteSpace(request.Assignee) ? "Sem responsavel" : request.Assignee.Trim(),
        Done = false
    };

    var updated = store.AddTask(partyId, task);
    return updated is null
        ? Results.NotFound()
        : Results.Created($"/api/parties/{partyId}", updated);
});

app.MapPost("/api/parties/{partyId:guid}/guests", (Guid partyId, CreateGuestRequest request, PartyStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["name"] = ["Name is required."]
        });
    }

    var guest = new Guest
    {
        Id = Guid.NewGuid(),
        Name = request.Name.Trim(),
        Group = string.IsNullOrWhiteSpace(request.Group) ? "Geral" : request.Group.Trim(),
        Status = string.IsNullOrWhiteSpace(request.Status) ? "Pendente" : request.Status.Trim()
    };

    var updated = store.AddGuest(partyId, guest);
    return updated is null
        ? Results.NotFound()
        : Results.Created($"/api/parties/{partyId}", updated);
});

app.MapPost("/api/parties/{partyId:guid}/budget-items", (Guid partyId, CreateBudgetItemRequest request, PartyStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.Label) || request.Amount <= 0)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["budgetItem"] = ["Label and positive amount are required."]
        });
    }

    var item = new BudgetItem
    {
        Id = Guid.NewGuid(),
        Label = request.Label.Trim(),
        Category = string.IsNullOrWhiteSpace(request.Category) ? "Geral" : request.Category.Trim(),
        Amount = request.Amount
    };

    var updated = store.AddBudgetItem(partyId, item);
    return updated is null
        ? Results.NotFound()
        : Results.Created($"/api/parties/{partyId}", updated);
});

app.Run();
