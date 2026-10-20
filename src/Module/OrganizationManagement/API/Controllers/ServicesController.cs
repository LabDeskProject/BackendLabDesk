using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.Application.Slices.Services.CreateService;
using OrganizationManagement.Application.Slices.Services.GetService;
using OrganizationManagement.Application.Slices.Services.ListOrganizationServices;

namespace OrganizationManagement.API.Controllers;

[ApiController]
[Route("api/services")]
[Authorize]
public sealed class ServicesController(ISender sender) : ControllerBase
{
    public sealed record CreateServiceRequest(Guid OrganizationId, Guid ResponsibleTeamId, string Name, string? Description);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceRequest request, CancellationToken ct)
    {
        var id = await sender.Send(new CreateServiceCommand(request.OrganizationId, request.ResponsibleTeamId, request.Name, request.Description), ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { serviceId = id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(new GetServiceQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-organization/{organizationId:guid}")]
    public async Task<IActionResult> ListByOrganization(Guid organizationId, CancellationToken ct) =>
        Ok(await sender.Send(new ListOrganizationServicesQuery(organizationId), ct));
}
