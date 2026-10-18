using Application.Register;
using AutoMapper;
using IdentityAccess.API.Contracts.Register;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAccess.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class RegisterController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public RegisterController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [ProducesResponseType(
            typeof(RegisterUserResponseDTO),
            StatusCodes.Status201Created)]
        [ProducesResponseType(
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserRequestDTO request,
            CancellationToken cancellationToken)
        {
            var command = _mapper.Map<RegisterUserCommand>(request);

            var result = await _sender.Send(command, cancellationToken);

            var response = _mapper.Map<RegisterUserResponseDTO>(result);

            return Created(
                $"/api/users/{result.UserId}",
                response);
        }
    }
}
