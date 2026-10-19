using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.Application.Slices;
using DomainCore.Organizations;
namespace OrganizationManagement.API.Controllers;
[ApiController, Authorize, Route("api/organizations/{organizationId:guid}/members")]
public sealed class MembershipController(ISender sender) : ControllerBase
{
    public sealed record AddMemberBody(Guid UserId, OrganizationRole Role);
    public sealed record ChangeRoleBody(OrganizationRole Role);
    [HttpPost]
    public async Task<IActionResult> Add(Guid organizationId, AddMemberBody body, CancellationToken ct)
    {
        await sender.Send(new AddOrganizationMemberCommand(organizationId, body.UserId, body.Role), ct);
        return NoContent();
    }
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> Remove(Guid organizationId, Guid userId, CancellationToken ct)
    {
        await sender.Send(new RemoveOrganizationMemberCommand(organizationId, userId), ct);
        return NoContent();
    }
    [HttpPut("{userId:guid}/role")]
    public async Task<IActionResult> ChangeRole(Guid organizationId, Guid userId, ChangeRoleBody body, CancellationToken ct)
    {
        await sender.Send(new ChangeOrganizationMemberRoleCommand(organizationId, userId, body.Role), ct);
        return NoContent();
    }
}
