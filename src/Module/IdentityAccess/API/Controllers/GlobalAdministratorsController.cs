using Application.Interface;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
namespace IdentityAccess.API.Controllers;
[ApiController, Route("api/admin/global-administrators")]
public sealed class GlobalAdministratorsController(IUserRepository users, IUnitOfWorkIdentityAccess unit, IConfiguration configuration) : ControllerBase
{
    public sealed record SetGlobalAdminBody(bool IsGlobalAdmin);
    [AllowAnonymous, HttpPost("bootstrap")]
    public async Task<IActionResult> Bootstrap(CancellationToken ct)
    {
        var expected = configuration["Security:BootstrapAdminEmail"];
        var supplied = Request.Headers["X-Bootstrap-Secret"].ToString();
        var configuredSecret = configuration["Security:BootstrapAdminSecret"];

        if (string.IsNullOrWhiteSpace(expected) || string.IsNullOrWhiteSpace(configuredSecret)) 
            return NotFound();

        var suppliedBytes = Encoding.UTF8.GetBytes(supplied); var configuredBytes = Encoding.UTF8.GetBytes(configuredSecret);
        if (suppliedBytes.Length != configuredBytes.Length || !CryptographicOperations.FixedTimeEquals(suppliedBytes, configuredBytes)) 
            return NotFound();

        await users.BootstrapGlobalAdminAsync(expected, ct);
        return NoContent();
    }
    [Authorize(Roles = "Admin"), HttpPut("{userId:guid}")]
    public async Task<IActionResult> Set(Guid userId, SetGlobalAdminBody body, CancellationToken ct)
    {
        await users.SetGlobalAdminAsync(userId, body.IsGlobalAdmin, ct);
        await unit.SaveChangesAsync(ct);
        return NoContent();
    }
}
