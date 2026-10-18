using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Register
{
    public sealed class RegisterUserCommandValidator
    : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email format is invalid.")
                .MaximumLength(320);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must contain at least 8 characters.")
                .Matches("[A-Z]")
                .WithMessage(
                    "Password must contain at least one uppercase letter.")
                .Matches("[0-9]")
                .WithMessage(
                    "Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage(
                    "Password must contain at least one special character.");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .MaximumLength(150);
        }
    }
}
