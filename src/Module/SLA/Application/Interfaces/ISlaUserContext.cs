using System;
using System.Collections.Generic;
using System.Text;

namespace SLA.Application.Interfaces
{
    public interface ISlaUserContext 
    { 
        Guid UserId { get; } bool IsAdmin { get; } 
    }
}
