using Application.Login;
using AutoMapper;
using BuildingBlocks.Application;
using IdentityAccess.API.Contracts.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAccess.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        public AuthController(
            IMediator mediator,
            IMapper mapper,
            IUserContext userContext)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userContext = userContext;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(
            typeof(LoginUserResponseDto),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            typeof(ProblemDetails),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            typeof(ProblemDetails),
            StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromBody] LoginUserRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var command = _mapper.Map<LoginUserCommand>(request);

                var result = await _mediator.Send(
                    command,
                    cancellationToken);
                Response.Headers["Cache-Control"] = "no-store";
                Response.Headers["Pragma"] = "no-cache";

                var response =
                    _mapper.Map<LoginUserResponseDto>(result);

                return Ok(response);
            }
            catch (UnauthorizedAccessException exception)
            {
                return StatusCode(
                    StatusCodes.Status401Unauthorized,
                    new ProblemDetails
                    {
                        Title = "Authentication failed",
                        Detail = exception.Message,
                        Status = StatusCodes.Status401Unauthorized
                    });
            }
        }
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                UserId = _userContext.UserId,
                IsAuthenticated = _userContext.IsAuthenticated
            });
        }
    }
}
