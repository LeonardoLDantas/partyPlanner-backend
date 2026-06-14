using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyPlanner.Application.DTOs.Requests;
using PartyPlanner.Application.Parties.Commands.AddBudgetItem;
using PartyPlanner.Application.Parties.Commands.AddGuest;
using PartyPlanner.Application.Parties.Commands.AddTask;
using PartyPlanner.Application.Parties.Commands.CreateParty;
using PartyPlanner.Application.Parties.Commands.DeleteBudgetItem;
using PartyPlanner.Application.Parties.Commands.DeleteGuest;
using PartyPlanner.Application.Parties.Commands.DeleteParty;
using PartyPlanner.Application.Parties.Commands.DeleteTask;
using PartyPlanner.Application.Parties.Commands.ToggleTask;
using PartyPlanner.Application.Parties.Commands.UpdateBudgetItem;
using PartyPlanner.Application.Parties.Commands.UpdateParty;
using PartyPlanner.Application.Parties.Commands.UpdateTask;
using PartyPlanner.Application.Parties.Commands.SendInvitation;
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
    [RequestSizeLimit(52_428_800)] // 50 MB — suporta CoverImageUrl como base64
    public async Task<IActionResult> Create([FromBody] CreatePartyRequest request, CancellationToken cancellationToken)
    {
        var command = new CreatePartyCommand(GetUserId(), request.Name, request.Category, request.Date, request.Time, request.Location, request.CoverImageUrl, request.ExpectedGuests, request.EstimatedBudget, request.IsFinalized);
        var party = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = party.Id }, party);
    }

    [HttpPut("{id:guid}")]
    [RequestSizeLimit(52_428_800)] // 50 MB — suporta CoverImageUrl como base64
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
    public async Task<IActionResult> UpdateTaskStatus(Guid partyId, Guid taskId, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
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

    [HttpPost("{partyId:guid}/guests")]
    public async Task<IActionResult> AddGuest(Guid partyId, [FromBody] CreateGuestRequest request, CancellationToken cancellationToken)
    {
        var command = new AddGuestCommand(GetUserId(), partyId, request.Name, request.Group, request.Type, request.Email, request.PhoneNumber);
        var party = await mediator.Send(command, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpDelete("{partyId:guid}/guests/{guestId:guid}")]
    public async Task<IActionResult> DeleteGuest(Guid partyId, Guid guestId, CancellationToken cancellationToken)
    {
        var party = await mediator.Send(new DeleteGuestCommand(GetUserId(), partyId, guestId), cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPost("{partyId:guid}/guests/{guestId:guid}/send-invitation")]
    public async Task<IActionResult> SendInvitation(Guid partyId, Guid guestId, CancellationToken cancellationToken)
    {
        await mediator.Send(new SendInvitationCommand(GetUserId(), partyId, guestId), cancellationToken);
        return NoContent();
    }

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
