using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace LabDesk.SeedWork.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        Guid? OrganizationId { get; }
        string? Role { get; }

    }
}
