using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLA.Application;
using SLA.Application.Slices.ArchiveSlaPolicy;
using SLA.Application.Slices.CreateSlaPolicy;
using SLA.Application.Slices.ListSlaBreaches;
using SLA.Application.Slices.ListSlaPolicies;
using SLA.Application.Slices.RecordSlaResponse;
using SLA.Application.Slices.ResolveSlaClock;
using SLA.Application.Slices.StartSlaClock;
using SLA.Application.Slices.UpdateSlaPolicy;
namespace SLA.API.Controllers;
[ApiController, Authorize, Route("api/sla")]
public sealed class SlaController(ISender sender) : ControllerBase
{
    public sealed record PolicyBody(Guid OrganizationId, Guid ServiceId, int ResponseMinutes, int ResolutionMinutes);
    public sealed record UpdatePolicyBody(int ResponseMinutes, int ResolutionMinutes);
    public sealed record ClockBody(Guid OrganizationId, Guid RequestId, Guid ServiceId);
    [HttpPost("policies")] 
    public async Task<IActionResult> CreatePolicy(PolicyBody b, CancellationToken ct) 
    { 
        var id = await sender.Send(new CreateSlaPolicyCommand(b.OrganizationId, b.ServiceId, b.ResponseMinutes, b.ResolutionMinutes), ct); 
        return Created($"api/sla/policies/{id}", new { policyId = id }); 
    }
    [HttpPut("policies/{id:guid}")] 
    public async Task<IActionResult> UpdatePolicy(Guid id, UpdatePolicyBody b, CancellationToken ct) 
    { 
        await sender.Send(new UpdateSlaPolicyCommand(id, b.ResponseMinutes, b.ResolutionMinutes), ct); 
        return NoContent(); 
    }
    [HttpDelete("policies/{id:guid}")] 
    public async Task<IActionResult> ArchivePolicy(Guid id, CancellationToken ct) 
    { 
        await sender.Send(new ArchiveSlaPolicyCommand(id), ct); 
        return NoContent(); 
    }
    [HttpGet("policies/{organizationId:guid}")] 
    public async Task<IActionResult> Policies(Guid organizationId, CancellationToken ct) => Ok(await sender.Send(new ListSlaPoliciesQuery(organizationId), ct));
    [HttpPost("clocks")] 
    public async Task<IActionResult> StartClock(ClockBody b, CancellationToken ct) 
    { 
        var id = await sender.Send(new StartSlaClockCommand(b.OrganizationId, b.RequestId, b.ServiceId), ct); 
        return Created($"api/sla/clocks/{id}", new { clockId = id }); 
    }
    [HttpPost("clocks/{id:guid}/response")] 
    public async Task<IActionResult> RecordResponse(Guid id, CancellationToken ct) 
    { 
        await sender.Send(new RecordSlaResponseCommand(id), ct); 
        return NoContent(); 
    }
    [HttpPost("clocks/{id:guid}/resolve")] 
    public async Task<IActionResult> Resolve(Guid id, CancellationToken ct) 
    { 
        await sender.Send(new ResolveSlaClockCommand(id), ct); 
        return NoContent(); 
    }
    [HttpGet("breaches/{organizationId:guid}")] 
    public async Task<IActionResult> Breaches(Guid organizationId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default) => Ok(await sender.Send(new ListSlaBreachesQuery(organizationId, page, pageSize), ct));
}
