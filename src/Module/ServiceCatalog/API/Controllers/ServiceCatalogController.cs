using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceCatalog.Application;
using ServiceCatalog.Application.Slices.ArchiveCatalogService;
using ServiceCatalog.Application.Slices.CreateCatalogService;
using ServiceCatalog.Application.Slices.GetCatalogService;
using ServiceCatalog.Application.Slices.ListCatalogServices;
using ServiceCatalog.Application.Slices.RenameCatalogService;

namespace ServiceCatalog.API.Controllers;

[ApiController]
[Authorize]
[Route("api/catalog/services")]
public sealed class ServiceCatalogController(ISender sender) : ControllerBase
{
    public sealed record CreateRequest(Guid OrganizationId, string Name, string? Description);
    public sealed record RenameRequest(string Name, string? Description);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequest request, CancellationToken ct)
    {
        var id = await sender.Send(new CreateCatalogServiceCommand(request.OrganizationId, request.Name, request.Description), ct);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Rename(Guid id, [FromBody] RenameRequest request, CancellationToken ct)
    {
        await sender.Send(new RenameCatalogServiceCommand(id, request.Name, request.Description), ct);
        return NoContent();
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct)
    {
        await sender.Send(new ArchiveCatalogServiceCommand(id), ct);
        return NoContent();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var item = await sender.Send(new GetCatalogServiceQuery(id), ct);
        return item is null ? NotFound() : Ok(item);
    }
    [HttpGet("by-organization/{organizationId:guid}")]
    public async Task<IActionResult> List(Guid organizationId, CancellationToken ct) => Ok(await sender.Send(new ListCatalogServicesQuery(organizationId), ct));
}
