using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityAccess.API.Contracts.Register
{
    public sealed record RegisterUserRequestDTO(
    string Email,
    string Password,
    string FullName);
}
