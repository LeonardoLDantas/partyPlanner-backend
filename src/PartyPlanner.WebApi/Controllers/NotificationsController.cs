using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyPlanner.Application.Interface;

namespace PartyPlanner.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class NotificationsController(INotificationService notificationService) : ControllerBase
{
    private Guid GetUserId()
    {
        var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(claimValue!);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var notifications = await notificationService.GetAllAsync(GetUserId(), cancellationToken);
        return Ok(notifications);
    }

    [HttpPatch("{notificationId:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification = await notificationService.MarkAsReadAsync(GetUserId(), notificationId, cancellationToken);
        return notification is null ? NotFound() : Ok(notification);
    }

    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var total = await notificationService.MarkAllAsReadAsync(GetUserId(), cancellationToken);
        return Ok(new { updated = total });
    }
}
