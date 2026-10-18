using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Register
{
    public sealed record RegisterUserResult(
    Guid UserId,
    string Message);
}
