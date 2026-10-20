using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Organizations.CreateOrganization
{
    public sealed record CreateOrganizationResult(Guid Id, string Name, string Slug, string Message);
}
