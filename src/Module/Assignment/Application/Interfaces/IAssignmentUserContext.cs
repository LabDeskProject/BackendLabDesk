using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Application.Interfaces
{
    public interface IAssignmentUserContext 
    {
        Guid UserId { get; } bool IsAdmin { get; } 
    }
}
