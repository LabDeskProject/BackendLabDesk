using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.DTOs
{
    public record OrganizationDto(
        Guid Id,
        string Name,
        string Slug,
        string? Description,
        DateTimeOffset CreatedAt);
}
