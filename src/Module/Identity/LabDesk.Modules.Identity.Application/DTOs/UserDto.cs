using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.DTOs
{
    public record UserDto(
    Guid Id,
    Guid OrganizationId,
    string Email,
    string FullName,
    string Role,
    Guid? TeamId,
    bool IsActive);

}
