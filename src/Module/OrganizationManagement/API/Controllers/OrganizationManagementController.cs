using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.Application.Slices.Organizations.CreateOrganization;
using OrganizationManagement.Application.Slices.Organizations.ListMyOrganizations;
using OrganizationManagement.Application.Slices.Organizations.ListOrganizationMembers;
using OrganizationManagement.Application.Slices.Teams.CreateTeam;

namespace OrganizationManagement.API.Controllers;

[ApiController]
[Route("api/organizations")]
[Authorize]
public sealed class OrganizationManagementController(ISender sender) : ControllerBase
{
    public sealed record CreateOrganizationRequest(string Name, string Slug, string? Description);
    public sealed record CreateTeamRequest(Guid OrganizationId, string Name, string? Description);

    [HttpPost]
    public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CreateOrganizationCommand(request.Name, request.Slug, request.Description), ct);
        return Created($"/api/organizations/{result.Id}", result);
    }

    [HttpGet("mine")]
    public async Task<IActionResult> Mine(CancellationToken ct) => Ok(await sender.Send(new ListMyOrganizationsQuery(), ct));

    [HttpGet("{organizationId:guid}/members")]
    public async Task<IActionResult> Members(Guid organizationId, CancellationToken ct) => Ok(await sender.Send(new ListOrganizationMembersQuery(organizationId), ct));

    [HttpPost("teams")]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CreateTeamCommand(request.OrganizationId, request.Name, request.Description), ct);
        return Created($"/api/teams/{result.Id}", result);
    }
}
