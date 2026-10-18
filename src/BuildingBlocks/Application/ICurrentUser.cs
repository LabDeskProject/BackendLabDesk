using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Application
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }

        string? Email { get; }

        bool IsAuthenticated { get; }
    }
}
