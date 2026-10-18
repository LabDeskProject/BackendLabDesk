using Application.RefreshTokens.RefreshToken;
using Application.RefreshTokens.RevokeRefreshToken;
using IdentityAccess.API.Contracts.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAccess.API.Controllers;

[ApiController]
[Route("api/auth/tokens")]
public sealed class RefreshTokensController : ControllerBase
{
    private readonly ISender _sender;
    public RefreshTokensController(ISender sender) => _sender = sender;

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(TokenPairResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
    {
        SetNoStore();
        try
        {
            var result = await _sender.Send(new RefreshTokenCommand(request.Token), cancellationToken);
            return Ok(new TokenPairResponseDto(result.AccessToken, result.RefreshToken, result.AccessTokenExpiresAtUtc));
        }
        catch (UnauthorizedAccessException)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new ProblemDetails { Title = "Invalid refresh token", Status = StatusCodes.Status401Unauthorized });
        }
    }

    [AllowAnonymous]
    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
    {
        SetNoStore();
        await _sender.Send(new RevokeRefreshTokenCommand(request.Token), cancellationToken);
        return NoContent();
    }

    private void SetNoStore()
    {
        Response.Headers["Cache-Control"] = "no-store";
        Response.Headers["Pragma"] = "no-cache";
    }
}
