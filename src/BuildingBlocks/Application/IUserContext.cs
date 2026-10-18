using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Application
{
    public interface IUserContext
    {
        Guid UserId { get; }

        bool IsAuthenticated { get; }
    }
}
