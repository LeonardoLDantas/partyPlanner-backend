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
    private const int MaximumExpectedGuests = 1_000_000;
    private const int MaximumPartyLocationLength = 150;
    private const decimal MaximumEstimatedBudget = 999_999_999_999m;

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

        if (request.ExpectedGuests is > MaximumExpectedGuests)
        {
            ModelState.AddModelError(nameof(request.ExpectedGuests), "Informe no maximo 1.000.000 de convidados esperados.");
            return ValidationProblem(ModelState);
        }

        if (!ValidatePartyLimits(request.Location, request.EstimatedBudget))
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var party = await partyService.CreateAsync(GetUserId(), request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = party.Id }, party);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(request.Date), exception.Message);
            return ValidationProblem(ModelState);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePartyRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            ModelState.AddModelError(nameof(request.Name), "Name is required.");
            return ValidationProblem(ModelState);
        }

        if (request.ExpectedGuests is > MaximumExpectedGuests)
        {
            ModelState.AddModelError(nameof(request.ExpectedGuests), "Informe no maximo 1.000.000 de convidados esperados.");
            return ValidationProblem(ModelState);
        }

        if (!ValidatePartyLimits(request.Location, request.EstimatedBudget))
        {
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        return await partyService.DeleteAsync(GetUserId(), id, cancellationToken) ? NoContent() : NotFound();
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

    [HttpPatch("{partyId:guid}/tasks/{taskId:guid}")]
    public async Task<IActionResult> UpdateTaskStatus(Guid partyId, Guid taskId, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var party = await partyService.UpdateTaskStatusAsync(GetUserId(), partyId, taskId, request, cancellationToken);
            return party is null ? NotFound() : Ok(party);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(partyId), exception.Message);
            return ValidationProblem(ModelState);
        }
    }

    [HttpDelete("{partyId:guid}/tasks/{taskId:guid}")]
    public async Task<IActionResult> DeleteTask(Guid partyId, Guid taskId, CancellationToken cancellationToken)
    {
        try
        {
            var party = await partyService.DeleteTaskAsync(GetUserId(), partyId, taskId, cancellationToken);
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

    [HttpDelete("{partyId:guid}/guests/{guestId:guid}")]
    public async Task<IActionResult> DeleteGuest(Guid partyId, Guid guestId, CancellationToken cancellationToken)
    {
        try
        {
            var party = await partyService.DeleteGuestAsync(GetUserId(), partyId, guestId, cancellationToken);
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

    [HttpPut("{partyId:guid}/budget-items/{budgetItemId:guid}")]
    public async Task<IActionResult> UpdateBudgetItem(Guid partyId, Guid budgetItemId, [FromBody] CreateBudgetItemRequest request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
        {
            ModelState.AddModelError(nameof(request.Amount), "Positive amount is required.");
            return ValidationProblem(ModelState);
        }

        try
        {
            var party = await partyService.UpdateBudgetItemAsync(GetUserId(), partyId, budgetItemId, request, cancellationToken);
            return party is null ? NotFound() : Ok(party);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(partyId), exception.Message);
            return ValidationProblem(ModelState);
        }
    }

    [HttpDelete("{partyId:guid}/budget-items/{budgetItemId:guid}")]
    public async Task<IActionResult> DeleteBudgetItem(Guid partyId, Guid budgetItemId, CancellationToken cancellationToken)
    {
        try
        {
            var party = await partyService.DeleteBudgetItemAsync(GetUserId(), partyId, budgetItemId, cancellationToken);
            return party is null ? NotFound() : Ok(party);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(nameof(partyId), exception.Message);
            return ValidationProblem(ModelState);
        }
    }

    private bool ValidatePartyLimits(string? location, decimal? estimatedBudget)
    {
        if (location?.Trim().Length > MaximumPartyLocationLength)
        {
            ModelState.AddModelError(nameof(location), "Informe um local com no maximo 150 caracteres.");
        }

        if (estimatedBudget is < 0 or > MaximumEstimatedBudget)
        {
            ModelState.AddModelError(nameof(estimatedBudget), "Informe um orcamento estimado entre zero e R$ 999.999.999.999,00.");
        }

        return ModelState.IsValid;
    }
}
