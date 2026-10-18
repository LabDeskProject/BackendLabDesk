using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityAccess.API.Contracts.Login
{
    public sealed class LoginUserRequestDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
