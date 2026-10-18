using System;
using System.Collections.Generic;
using System.Text;

namespace APICore.Contracts.Organizations
{
    public sealed record CreateOrganizationResponseDTO(
    Guid OrganizationId,
    string Name,
    string Slug,
    string Message);
}
