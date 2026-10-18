using BuildingBlocks.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Login
{
    public sealed record LoginUserCommand(
    string Email,
    string Password)
    : ICommand<LoginUserResult>;
}
