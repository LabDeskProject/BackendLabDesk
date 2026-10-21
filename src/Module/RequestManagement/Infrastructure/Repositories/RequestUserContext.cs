using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RequestManagement.Application.Interfaces;

namespace RequestManagement.Infrastructure;

public sealed class RequestUserContext(IHttpContextAccessor accessor) : IRequestUserContext
{
    public bool IsAdmin => accessor.HttpContext?.User.IsInRole("Admin") == true;
    public Guid UserId
    {
        get
        {
            var value = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var id) ? id : throw new UnauthorizedAccessException("Authenticated user ID claim is missing.");
        }
    }
}
