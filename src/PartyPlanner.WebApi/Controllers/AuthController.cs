using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyPlanner.Application.Auth.Commands.Login;
using PartyPlanner.Application.Auth.Commands.LoginWithGoogle;
using PartyPlanner.Application.Auth.Commands.Register;
using PartyPlanner.Application.Auth.Queries.GetProfile;
using PartyPlanner.Application.DTOs.Requests;

namespace PartyPlanner.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(request.Name, request.Email, request.Password);
        var response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    [HttpPost("google")]
    public async Task<IActionResult> Google([FromBody] GoogleLoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginWithGoogleCommand(request.IdToken);
        var response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var userId)) return Unauthorized();

        var query = new GetProfileQuery(userId);
        var user = await mediator.Send(query, cancellationToken);
        return user is null ? Unauthorized() : Ok(user);
    }
}
