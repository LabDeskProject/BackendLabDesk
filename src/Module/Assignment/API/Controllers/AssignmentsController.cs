using Assignment.Application;
using Assignment.Application.Slices.AssignRequest;
using Assignment.Application.Slices.ListMyAssignments;
using Assignment.Application.Slices.ListRequestAssignments;
using Assignment.Application.Slices.ReassignRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Assignment.API.Controllers;
[ApiController, Authorize, Route("api/assignments")]
public sealed class AssignmentsController(ISender sender) : ControllerBase
{
    public sealed record AssignBody(Guid OrganizationId, Guid RequestId, Guid AssigneeUserId, Guid? TeamId, string? Note);
    public sealed record ReassignBody(Guid AssigneeUserId, Guid? TeamId, string? Note);

    [HttpPost]
    public async Task<IActionResult> Assign(AssignBody body, CancellationToken ct) 
    { 
        var id = await sender.Send(new AssignRequestCommand(body.OrganizationId, body.RequestId, body.AssigneeUserId, body.TeamId, body.Note), ct); 
        return Created($"api/assignments/{id}", new { assignmentId = id }); 
    }

    [HttpPost("{id:guid}/reassign")]
    public async Task<IActionResult> Reassign(Guid id, ReassignBody body, CancellationToken ct) 
    { 
        var nextId = await sender.Send(new ReassignRequestCommand(id, body.AssigneeUserId, body.TeamId, body.Note), ct); 
        return Created($"api/assignments/{nextId}", new { assignmentId = nextId }); 
    }

    [HttpGet("by-request/{requestId:guid}")]
    public async Task<IActionResult> ByRequest(Guid requestId, CancellationToken ct) => Ok(await sender.Send(new ListRequestAssignmentsQuery(requestId), ct));

    [HttpGet("mine/{organizationId:guid}")]
    public async Task<IActionResult> Mine(Guid organizationId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default) => Ok(await sender.Send(new ListMyAssignmentsQuery(organizationId, page, pageSize), ct));
}
