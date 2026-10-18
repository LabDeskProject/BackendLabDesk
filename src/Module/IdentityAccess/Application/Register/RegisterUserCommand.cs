using BuildingBlocks.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Register
{
    public sealed record RegisterUserCommand(
     string Email,
     string Password,
     string FullName)
     : ICommand<RegisterUserResult>;
}
