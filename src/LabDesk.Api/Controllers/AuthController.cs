using Application.Configuration.Commands;
using Application.Configuration.Queries;
using Application.UserAccess.Commands;
using Application.UserAccess.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LabDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICommandHandler<RegisterUserCommand> _registerCommandHandler;
        private readonly IQueryHandler<LoginUserQuery, LoginResultDto> _loginQueryHandler;

        public AuthController(
            ICommandHandler<RegisterUserCommand> registerCommandHandler,
            IQueryHandler<LoginUserQuery, LoginResultDto> loginQueryHandler)
        {
            _registerCommandHandler = registerCommandHandler;
            _loginQueryHandler = loginQueryHandler;
        }

        /// <summary>
        /// Đăng ký user mới
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = new RegisterUserCommand(
                    request.Login,
                    request.Password,
                    request.Email,
                    request.FirstName,
                    request.LastName);

                await _registerCommandHandler.Handle(command, cancellationToken);

                return Ok(new { message = "User đã được đăng ký thành công" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi server", detail = ex.Message });
            }
        }

        /// <summary>
        /// Đăng nhập user
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var query = new LoginUserQuery(request.Login, request.Password);

                var result = await _loginQueryHandler.Handle(query, cancellationToken);

                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    data = new
                    {
                        userId = result.UserId,
                        login = result.Login,
                        email = result.Email,
                        fullName = result.FullName,
                        role = result.Role,
                        jwtToken = result.JwtToken
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi server", detail = ex.Message });
            }
        }
    }

    /// <summary>
    /// Request model cho register
    /// </summary>
    public class RegisterRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    /// <summary>
    /// Request model cho login
    /// </summary>
    public class LoginRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
