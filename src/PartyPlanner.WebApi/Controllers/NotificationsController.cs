using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyPlanner.Application.Notifications.Commands.ClearAll;
using PartyPlanner.Application.Notifications.Commands.MarkAllAsRead;
using PartyPlanner.Application.Notifications.Commands.MarkAsRead;
using PartyPlanner.Application.Notifications.Queries.GetNotifications;

namespace PartyPlanner.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class NotificationsController(IMediator mediator) : ControllerBase
{
    private Guid GetUserId()
    {
        var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(claimValue!);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var notifications = await mediator.Send(new GetNotificationsQuery(GetUserId()), cancellationToken);
        return Ok(notifications);
    }

    [HttpPatch("{notificationId:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification = await mediator.Send(new MarkAsReadCommand(GetUserId(), notificationId), cancellationToken);
        return notification is null ? NotFound() : Ok(notification);
    }

    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var total = await mediator.Send(new MarkAllAsReadCommand(GetUserId()), cancellationToken);
        return Ok(new { updated = total });
    }

    [HttpDelete]
    public async Task<IActionResult> ClearAll(CancellationToken cancellationToken)
    {
        var total = await mediator.Send(new ClearAllCommand(GetUserId()), cancellationToken);
        return Ok(new { deleted = total });
    }
}
