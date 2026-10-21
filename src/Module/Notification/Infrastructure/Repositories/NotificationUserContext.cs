using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Notification.Application;
using Notification.Application.Interfaces;
namespace Notification.Infrastructure.Repositories;
public sealed class NotificationUserContext(IHttpContextAccessor accessor) : INotificationUserContext
{
    public Guid UserId => Guid.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : throw new UnauthorizedAccessException("Authenticated user ID claim is missing.");
}
