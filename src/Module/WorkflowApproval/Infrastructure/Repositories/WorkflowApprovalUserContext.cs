using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WorkflowApproval.Application;
using WorkflowApproval.Application.Interfaces;

namespace WorkflowApproval.Infrastructure.Repositories;

public sealed class WorkflowApprovalUserContext(IHttpContextAccessor accessor) : IApprovalUserContext
{
    public bool IsAdmin => accessor.HttpContext?.User.IsInRole("Admin") == true;
    public Guid UserId => Guid.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : throw new UnauthorizedAccessException("Authenticated user ID claim is missing.");
}
