using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyPlanner.Application.DTOs.Requests;
using PartyPlanner.Application.Parties.Commands.AddBudgetItem;
using PartyPlanner.Application.Parties.Commands.AddGuestToConvite;
using PartyPlanner.Application.Parties.Commands.AddTask;
using PartyPlanner.Application.Parties.Commands.CreateConvite;
using PartyPlanner.Application.Parties.Commands.CreateParty;
using PartyPlanner.Application.Parties.Commands.DeleteBudgetItem;
using PartyPlanner.Application.Parties.Commands.DeleteConvite;
using PartyPlanner.Application.Parties.Commands.DeleteGuestFromConvite;
using PartyPlanner.Application.Parties.Commands.DeleteParty;
using PartyPlanner.Application.Parties.Commands.DeleteTask;
using PartyPlanner.Application.Parties.Commands.ToggleTask;
using PartyPlanner.Application.Parties.Commands.UpdateBudgetItem;
using PartyPlanner.Application.Parties.Commands.UpdateConvite;
using PartyPlanner.Application.Parties.Commands.UpdateGuest;
using PartyPlanner.Application.Parties.Commands.UpdateParty;
using PartyPlanner.Application.Parties.Commands.UpdateTask;
using PartyPlanner.Application.Parties.Queries.GetAllParties;
using PartyPlanner.Application.Parties.Queries.GetPartyById;

namespace PartyPlanner.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class PartiesController(IMediator mediator) : ControllerBase
{
    private Guid GetUserId()
    {
        var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(claimValue!);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var parties = await mediator.Send(new GetAllPartiesQuery(GetUserId()), cancellationToken);
        return Ok(parties);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var party = await mediator.Send(new GetPartyByIdQuery(id, GetUserId()), cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPost]
    [RequestSizeLimit(52_428_800)]
    public async Task<IActionResult> Create([FromBody] CreatePartyRequest request, CancellationToken cancellationToken)
    {
        var command = new CreatePartyCommand(GetUserId(), request.Name, request.Category, request.Date, request.Time, request.Location, request.CoverImageUrl, request.ExpectedGuests, request.EstimatedBudget, request.IsFinalized);
        var party = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = party.Id }, party);
    }

    [HttpPut("{id:guid}")]
    [RequestSizeLimit(52_428_800)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePartyRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdatePartyCommand(GetUserId(), id, request.Name, request.Category, request.Date, request.Time, request.Location, request.CoverImageUrl, request.ExpectedGuests, request.EstimatedBudget, request.IsFinalized);
        var party = await mediator.Send(command, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeletePartyCommand(GetUserId(), id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    // ── Tasks ──────────────────────────────────────────────────────────────

    [HttpPost("{partyId:guid}/tasks")]
    public async Task<IActionResult> AddTask(Guid partyId, [FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var command = new AddTaskCommand(GetUserId(), partyId, request.Title, request.Assignee, request.DueDate, request.Description, request.Status);
        var party = await mediator.Send(command, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPatch("{partyId:guid}/tasks/{taskId:guid}/toggle")]
    public async Task<IActionResult> ToggleTask(Guid partyId, Guid taskId, CancellationToken cancellationToken)
    {
        var party = await mediator.Send(new ToggleTaskCommand(GetUserId(), partyId, taskId), cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPatch("{partyId:guid}/tasks/{taskId:guid}")]
    public async Task<IActionResult> UpdateTask(Guid partyId, Guid taskId, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTaskCommand(GetUserId(), partyId, taskId, request.Title, request.Assignee, request.Description, request.Status);
        var party = await mediator.Send(command, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpDelete("{partyId:guid}/tasks/{taskId:guid}")]
    public async Task<IActionResult> DeleteTask(Guid partyId, Guid taskId, CancellationToken cancellationToken)
    {
        var party = await mediator.Send(new DeleteTaskCommand(GetUserId(), partyId, taskId), cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    // ── Convites ───────────────────────────────────────────────────────────

    [HttpPost("{partyId:guid}/convites")]
    public async Task<IActionResult> CreateConvite(Guid partyId, [FromBody] CreateConviteRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateConviteCommand(GetUserId(), partyId, request.Nome, request.Observacao, request.Tipo, request.QuantidadeSenhas, request.SenhaPresente);
        var party = await mediator.Send(command, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPut("{partyId:guid}/convites/{conviteId:guid}")]
    public async Task<IActionResult> UpdateConvite(Guid partyId, Guid conviteId, [FromBody] UpdateConviteRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateConviteCommand(GetUserId(), partyId, conviteId, request.Nome, request.Observacao, request.Tipo, request.SenhaPresente);
        var party = await mediator.Send(command, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpDelete("{partyId:guid}/convites/{conviteId:guid}")]
    public async Task<IActionResult> DeleteConvite(Guid partyId, Guid conviteId, CancellationToken cancellationToken)
    {
        var party = await mediator.Send(new DeleteConviteCommand(GetUserId(), partyId, conviteId), cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    // ── Guests (dentro de Convite) ──────────────────────────────────────────

    [HttpPost("{partyId:guid}/convites/{conviteId:guid}/guests")]
    public async Task<IActionResult> AddGuest(Guid partyId, Guid conviteId, [FromBody] CreateGuestRequest request, CancellationToken cancellationToken)
    {
        var command = new AddGuestToConviteCommand(GetUserId(), partyId, conviteId, request.Name, request.Group, request.Type, request.Email, request.PhoneNumber);
        var party = await mediator.Send(command, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPut("{partyId:guid}/convites/{conviteId:guid}/guests/{guestId:guid}")]
    public async Task<IActionResult> UpdateGuest(Guid partyId, Guid conviteId, Guid guestId, [FromBody] UpdateGuestRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateGuestCommand(GetUserId(), partyId, conviteId, guestId, request.Name, request.Group, request.Type, request.Email, request.PhoneNumber);
        var party = await mediator.Send(command, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpDelete("{partyId:guid}/convites/{conviteId:guid}/guests/{guestId:guid}")]
    public async Task<IActionResult> DeleteGuest(Guid partyId, Guid conviteId, Guid guestId, CancellationToken cancellationToken)
    {
        var party = await mediator.Send(new DeleteGuestFromConviteCommand(GetUserId(), partyId, conviteId, guestId), cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    // ── Budget Items ───────────────────────────────────────────────────────

    [HttpPost("{partyId:guid}/budget-items")]
    public async Task<IActionResult> AddBudgetItem(Guid partyId, [FromBody] CreateBudgetItemRequest request, CancellationToken cancellationToken)
    {
        var command = new AddBudgetItemCommand(GetUserId(), partyId, request.Label, request.Category, request.Amount, request.IsPaid);
        var party = await mediator.Send(command, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPut("{partyId:guid}/budget-items/{budgetItemId:guid}")]
    public async Task<IActionResult> UpdateBudgetItem(Guid partyId, Guid budgetItemId, [FromBody] CreateBudgetItemRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateBudgetItemCommand(GetUserId(), partyId, budgetItemId, request.Amount, request.IsPaid);
        var party = await mediator.Send(command, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpDelete("{partyId:guid}/budget-items/{budgetItemId:guid}")]
    public async Task<IActionResult> DeleteBudgetItem(Guid partyId, Guid budgetItemId, CancellationToken cancellationToken)
    {
        var party = await mediator.Send(new DeleteBudgetItemCommand(GetUserId(), partyId, budgetItemId), cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }
}
