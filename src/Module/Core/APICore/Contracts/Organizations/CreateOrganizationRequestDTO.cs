using System;
using System.Collections.Generic;
using System.Text;

namespace APICore.Contracts.Organizations
{
    public sealed record CreateOrganizationRequestDTO(
    string Name,
    string Slug,
    string? Description);
}
