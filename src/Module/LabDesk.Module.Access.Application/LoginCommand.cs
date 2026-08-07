using LabDesk.SeedWork.Application.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Module.Access.Application
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<string>>;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthTokenGenerator _tokenGenerator;

        public LoginCommandHandler(IUserRepository userRepository, IAuthTokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null || !IsPasswordValid(request.Password, user.PasswordHash))
            {
                return Result.Failure<string>("Email hoặc mật khẩu không chính xác.");
            }

            // Sinh JWT token trả về cho client
            var token = _tokenGenerator.GenerateAccessToken(user);
            return Result.Success(token);
        }

        private bool IsPasswordValid(string plainPassword, string passwordHash)
        {
            // Logic kiểm tra mật khẩu (ví dụ: BCrypt.Net verify)
            return BCrypt.Net.BCrypt.Verify(plainPassword, passwordHash);
        }
    }
}
