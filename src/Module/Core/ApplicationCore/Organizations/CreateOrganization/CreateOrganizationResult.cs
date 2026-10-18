using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Organizations.CreateOrganization
{
    public sealed record CreateOrganizationResult(
    Guid OrganizationId,
    string Name,
    string Slug,
    string Message);
}
