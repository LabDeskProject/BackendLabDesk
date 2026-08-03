using LabDesk.Modules.Identity.Application.DTOs;
using LabDesk.SeedWork.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Queries.GetOrganizationBySlug
{
    public record GetOrganizationBySlugQuery(string Slug) : IQuery<OrganizationDto?>;
}
