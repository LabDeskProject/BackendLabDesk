using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityAccess.API.Contracts.Register
{
    public sealed record RegisterUserResponseDTO(
    Guid UserId,
    string Message);
}
