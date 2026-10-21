using System;
using System.Collections.Generic;
using System.Text;

namespace RequestManagement.Application.Interfaces
{

    public interface IRequestUserContext 
    { 
        Guid UserId { get; } bool IsAdmin { get; } 
    }
}
