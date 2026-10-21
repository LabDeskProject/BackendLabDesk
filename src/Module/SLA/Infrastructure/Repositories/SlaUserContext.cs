using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SLA.Application;
using SLA.Application.Interfaces;
namespace SLA.Infrastructure.Repositories;
public sealed class SlaUserContext(IHttpContextAccessor accessor) : ISlaUserContext
{
    public Guid UserId => Guid.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : throw new UnauthorizedAccessException("Authenticated user ID claim is missing.");
    public bool IsAdmin => accessor.HttpContext?.User.IsInRole("Admin") == true;
}
