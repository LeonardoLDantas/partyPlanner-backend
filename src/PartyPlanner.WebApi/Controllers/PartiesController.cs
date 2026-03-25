using Microsoft.AspNetCore.Mvc;
using PartyPlanner.Application.Interface;
using PartyPlanner.Core.DTO.Requests;

namespace PartyPlanner.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PartiesController(IPartyService partyService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var parties = await partyService.GetAllAsync(cancellationToken);
        return Ok(parties);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var party = await partyService.GetByIdAsync(id, cancellationToken);
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

        var party = await partyService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = party.Id }, party);
    }

    [HttpPost("{partyId:guid}/tasks")]
    public async Task<IActionResult> AddTask(Guid partyId, [FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            ModelState.AddModelError(nameof(request.Title), "Title is required.");
            return ValidationProblem(ModelState);
        }

        var party = await partyService.AddTaskAsync(partyId, request, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPatch("{partyId:guid}/tasks/{taskId:guid}/toggle")]
    public async Task<IActionResult> ToggleTask(Guid partyId, Guid taskId, CancellationToken cancellationToken)
    {
        var party = await partyService.ToggleTaskAsync(partyId, taskId, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPost("{partyId:guid}/guests")]
    public async Task<IActionResult> AddGuest(Guid partyId, [FromBody] CreateGuestRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            ModelState.AddModelError(nameof(request.Name), "Name is required.");
            return ValidationProblem(ModelState);
        }

        var party = await partyService.AddGuestAsync(partyId, request, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }

    [HttpPost("{partyId:guid}/budget-items")]
    public async Task<IActionResult> AddBudgetItem(Guid partyId, [FromBody] CreateBudgetItemRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Label) || request.Amount <= 0)
        {
            ModelState.AddModelError(nameof(request.Label), "Label and positive amount are required.");
            return ValidationProblem(ModelState);
        }

        var party = await partyService.AddBudgetItemAsync(partyId, request, cancellationToken);
        return party is null ? NotFound() : Ok(party);
    }
}
