using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkflowApproval.Application;
using WorkflowApproval.Application.Slices.DecideApproval;
using WorkflowApproval.Application.Slices.GetApprovalWorkflow;
using WorkflowApproval.Application.Slices.StartApprovalWorkflow;
using WorkflowApproval.Domain;
using WorkflowApproval.Domain.Enum;

namespace WorkflowApproval.API.Controllers;

[ApiController]
[Authorize]
[Route("api/approval-workflows")]
public sealed class ApprovalWorkflowsController(ISender sender) : ControllerBase
{
    public sealed record StartBody(Guid OrganizationId, Guid RequestId, ApprovalMode Mode, IReadOnlyList<IReadOnlyList<Guid>> ApproverSteps);
    public sealed record DecideBody(ApprovalDecision Decision, string? Comment);

    [HttpPost]
    public async Task<IActionResult> Start(StartBody body, CancellationToken ct)
    {
        var id = await sender.Send(new StartApprovalWorkflowCommand(body.OrganizationId, body.RequestId, body.Mode, body.ApproverSteps), ct);
        return CreatedAtAction(nameof(Get), new { id }, new { workflowId = id });
    }
    [HttpPost("{id:guid}/decision")]
    public async Task<IActionResult> Decide(Guid id, DecideBody body, CancellationToken ct)
    {
        await sender.Send(new DecideApprovalCommand(id, body.Decision, body.Comment), ct);
        return NoContent();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var workflow = await sender.Send(new GetApprovalWorkflowQuery(id), ct);
        return workflow is null ? NotFound() : Ok(workflow);
    }
}
