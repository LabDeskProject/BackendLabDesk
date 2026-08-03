using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.DTOs
{
    public record TeamDto(
            Guid Id,
            Guid OrganizationId,
            string Name,
            string? Description,
            Guid? TeamLeadId);
    
}
