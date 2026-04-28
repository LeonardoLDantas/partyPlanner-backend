using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyPlanner.Application.Interface;
using PartyPlanner.Core.DTO.Requests;

namespace PartyPlanner.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class PartiesController(IPartyService partyService) : ControllerBase
{
    private Guid GetUserId()
    {
        var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(claimValue!);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var parties = await partyService.GetAllAsync(GetUserId(), cancellationToken);
        return Ok(parties);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var party = await partyService.GetByIdAsync(id, GetUserId(), cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePartyRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            ModelState.AddModelError(nameof(request.Name), "Name is required.");
            return ValidationProblem(ModelState);
        }

        var party = await partyService.CreateAsync(GetUserId(), request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = party.Id }, party);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePartyRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            ModelState.AddModelError(nameof(request.Name), "Name is required.");
            return ValidationProblem(ModelState);
        }

        try
        {
            var party = await partyService.UpdateAsync(GetUserId(), id, request, cancellationToken);
            return party is null ? NotFound() : Ok(party);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(id), exception.Message);
            return ValidationProblem(ModelState);
        }
    }

    [HttpPost("{partyId:guid}/tasks")]
    public async Task<IActionResult> AddTask(Guid partyId, [FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            ModelState.AddModelError(nameof(request.Title), "Title is required.");
            return ValidationProblem(ModelState);
        }

        try
        {
            var party = await partyService.AddTaskAsync(GetUserId(), partyId, request, cancellationToken);
            return party is null ? NotFound() : Ok(party);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(partyId), exception.Message);
            return ValidationProblem(ModelState);
        }
    }

    [HttpPatch("{partyId:guid}/tasks/{taskId:guid}/toggle")]
    public async Task<IActionResult> ToggleTask(Guid partyId, Guid taskId, CancellationToken cancellationToken)
    {
        try
        {
            var party = await partyService.ToggleTaskAsync(GetUserId(), partyId, taskId, cancellationToken);
            return party is null ? NotFound() : Ok(party);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(partyId), exception.Message);
            return ValidationProblem(ModelState);
        }
    }

    [HttpPost("{partyId:guid}/guests")]
    public async Task<IActionResult> AddGuest(Guid partyId, [FromBody] CreateGuestRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            ModelState.AddModelError(nameof(request.Name), "Name is required.");
            return ValidationProblem(ModelState);
        }

        try
        {
            var party = await partyService.AddGuestAsync(GetUserId(), partyId, request, cancellationToken);
            return party is null ? NotFound() : Ok(party);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(partyId), exception.Message);
            return ValidationProblem(ModelState);
        }
    }

    [HttpPost("{partyId:guid}/budget-items")]
    public async Task<IActionResult> AddBudgetItem(Guid partyId, [FromBody] CreateBudgetItemRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Label) || request.Amount <= 0)
        {
            ModelState.AddModelError(nameof(request.Label), "Label and positive amount are required.");
            return ValidationProblem(ModelState);
        }

        try
        {
            var party = await partyService.AddBudgetItemAsync(GetUserId(), partyId, request, cancellationToken);
            return party is null ? NotFound() : Ok(party);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(partyId), exception.Message);
            return ValidationProblem(ModelState);
        }
    }
}
