using LabDesk.Modules.Identity.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace LabDesk.Module.Access.Application
{
    public interface IAuthTokenGenerator
    {
        string GenerateAuthToken(User user);
    }
}
