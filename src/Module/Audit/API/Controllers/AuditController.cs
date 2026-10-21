using Audit.Application;
using Audit.Application.Slices.ListEntityAudit;
using Audit.Application.Slices.ListOrganizationAudit;
using Audit.Application.Slices.RecordAuditEntry;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Audit.API.Controllers;
[ApiController, Authorize, Route("api/audit")]
public sealed class AuditController(ISender sender) : ControllerBase
{
    public sealed record RecordBody(Guid OrganizationId, Guid? ActorUserId, string Action, string EntityType, string EntityId, string? CorrelationId, string? DetailsJson);
    [HttpPost("entries")]
    public async Task<IActionResult> Record(RecordBody b, CancellationToken ct) 
    { 
        var id = await sender.Send(new RecordAuditEntryCommand(b.OrganizationId, b.ActorUserId, b.Action, b.EntityType, b.EntityId, b.CorrelationId, b.DetailsJson), ct); 
        return Created($"api/audit/entries/{id}", new { auditEntryId = id }); 
    }
    [HttpGet("organizations/{organizationId:guid}")]
    public async Task<IActionResult> Organization(Guid organizationId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default) => Ok(await sender.Send(new ListOrganizationAuditQuery(organizationId, page, pageSize), ct));
    [HttpGet("organizations/{organizationId:guid}/entities/{entityType}/{entityId}")]
    public async Task<IActionResult> Entity(Guid organizationId, string entityType, string entityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default) => Ok(await sender.Send(new ListEntityAuditQuery(organizationId, entityType, entityId, page, pageSize), ct));
}
