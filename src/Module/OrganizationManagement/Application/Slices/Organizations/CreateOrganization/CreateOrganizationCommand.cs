using BuildingBlocks.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Organizations.CreateOrganization
{
    public sealed record CreateOrganizationCommand(string Name, string Slug, string? Description) : ICommand<CreateOrganizationResult>;
}
