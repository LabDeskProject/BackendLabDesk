using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequestManagement.Application;
using RequestManagement.Application.Slices.GetRequest;
using RequestManagement.Application.Slices.ListOrganizationRequests;
using RequestManagement.Application.Slices.SubmitRequest;

namespace RequestManagement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/requests")]
public sealed class RequestsController(ISender sender) : ControllerBase
{
    public sealed record SubmitRequestBody(Guid OrganizationId, Guid ServiceId, string Title, string? Description, string FormDataJson);

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitRequestBody body, CancellationToken ct)
    {
        var id = await sender.Send(new SubmitRequestCommand(body.OrganizationId, body.ServiceId, body.Title, body.Description, body.FormDataJson), ct);
        return CreatedAtAction(nameof(Get), new { id }, new { requestId = id });
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var item = await sender.Send(new GetRequestQuery(id), ct);
        return item is null ? NotFound() : Ok(item);
    }
    [HttpGet("by-organization/{organizationId:guid}")]
    public async Task<IActionResult> List(Guid organizationId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default) =>
        Ok(await sender.Send(new ListOrganizationRequestsQuery(organizationId, page, pageSize), ct));
}
