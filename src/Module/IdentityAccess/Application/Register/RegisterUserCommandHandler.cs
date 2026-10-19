using Application.Interface;
using Application.Interface.Security;
using Application.Register.Error;
using BuildingBlocks.Application;
using BuildingBlocks.Application.CQRS;
using Domain.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Register
{
    public sealed class RegisterUserCommandHandler
    : ICommandHandler<RegisterUserCommand, RegisterUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWorkIdentityAccess _unitOfWorkIdentityAccess;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IUnitOfWorkIdentityAccess unitOfWorkIdentityAccess)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWorkIdentityAccess = unitOfWorkIdentityAccess;
        }

        public async Task<RegisterUserResult> Handle(
            RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            var email = command.Email
                .Trim()
                .ToLowerInvariant();

            var emailExists =
                await _userRepository.ExistsByEmailAsync(
                    email,
                    cancellationToken);

            if (emailExists)
            {
                throw new UserAlreadyExistsException();
            }

            var passwordHash =
                _passwordHasher.Hash(command.Password);

            var user = User.Create(
                email,
                command.FullName,
                passwordHash);

            _userRepository.Add(user);

            await _unitOfWorkIdentityAccess.SaveChangesAsync(
                cancellationToken);

            return new RegisterUserResult(
                user.Id,
                "User registered successfully.");
        }
    }
}
