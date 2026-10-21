using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Audit.Application;
using Audit.Application.Interfaces;
namespace Audit.Infrastructure.Repositories;
public sealed class AuditUserContext(IHttpContextAccessor accessor) : IAuditUserContext
{
    public Guid UserId => Guid.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : throw new UnauthorizedAccessException("Authenticated user ID claim is missing.");
    public bool IsAdmin => accessor.HttpContext?.User.IsInRole("Admin") == true;
}
