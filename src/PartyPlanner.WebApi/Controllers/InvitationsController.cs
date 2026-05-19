using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyPlanner.Application.Interface;
using PartyPlanner.Core.DTO.Requests;

namespace PartyPlanner.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/invitations")]
public sealed class InvitationsController(IPartyService partyService) : ControllerBase
{
    [HttpGet("{token}")]
    public async Task<IActionResult> Get(string token, CancellationToken cancellationToken)
    {
        var invitation = await partyService.GetInvitationAsync(token, cancellationToken);
        return invitation is null ? NotFound() : Ok(invitation);
    }

    [HttpPost("{token}/respond")]
    public async Task<IActionResult> Respond(string token, [FromBody] RespondInvitationRequest request, CancellationToken cancellationToken)
    {
        if (!request.Status.Equals("Confirmado", StringComparison.OrdinalIgnoreCase) &&
            !request.Status.Equals("Recusou", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(nameof(request.Status), "Status must be Confirmado or Recusou.");
            return ValidationProblem(ModelState);
        }

        var invitation = await partyService.RespondInvitationAsync(token, request, cancellationToken);
        return invitation is null ? NotFound() : Ok(invitation);
    }
}
