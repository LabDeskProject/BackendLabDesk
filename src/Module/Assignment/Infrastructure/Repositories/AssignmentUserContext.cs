using Assignment.Application;
using Assignment.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace Assignment.Infrastructure.Repositories;
public sealed class AssignmentUserContext(IHttpContextAccessor accessor) : IAssignmentUserContext
{
    public bool IsAdmin => accessor.HttpContext?.User.IsInRole("Admin") == true;
    public Guid UserId => Guid.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : throw new UnauthorizedAccessException("Authenticated user ID claim is missing.");
}
