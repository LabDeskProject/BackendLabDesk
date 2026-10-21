using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Application;
using Notification.Application.Slices.ListMyNotifications;
using Notification.Application.Slices.MarkNotificationRead;
namespace Notification.API.Controllers;
[ApiController, Authorize, Route("api/notifications")]
public sealed class NotificationsController(ISender sender) : ControllerBase
{
    [HttpGet("mine/{organizationId:guid}")]
    public async Task<IActionResult> Mine(Guid organizationId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default) => Ok(await sender.Send(new ListMyNotificationsQuery(organizationId, page, pageSize), ct));

    [HttpPost("{id:guid}/read")]
    public async Task<IActionResult> MarkRead(Guid id, CancellationToken ct) => await sender.Send(new MarkNotificationReadCommand(id), ct) ? NoContent() : NotFound();
}
