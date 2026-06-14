using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyPlanner.Application.DTOs.Requests;
using PartyPlanner.Application.Parties.Commands.RespondInvitation;
using PartyPlanner.Application.Parties.Queries.GetInvitation;

namespace PartyPlanner.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/invitations")]
public sealed class InvitationsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{token}")]
    public async Task<IActionResult> Get(string token, CancellationToken cancellationToken)
    {
        var invitation = await mediator.Send(new GetInvitationQuery(token), cancellationToken);
        return invitation is null ? NotFound() : Ok(invitation);
    }

    [HttpPost("{token}/respond")]
    public async Task<IActionResult> Respond(string token, [FromBody] RespondInvitationRequest request, CancellationToken cancellationToken)
    {
        var command = new RespondInvitationCommand(token, request.Status);
        var invitation = await mediator.Send(command, cancellationToken);
        return invitation is null ? NotFound() : Ok(invitation);
    }
}
