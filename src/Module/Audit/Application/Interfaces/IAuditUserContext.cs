using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Interfaces
{
    public interface IAuditUserContext 
    { 
        Guid UserId { get; } 
        bool IsAdmin { get; } 
    }

}
