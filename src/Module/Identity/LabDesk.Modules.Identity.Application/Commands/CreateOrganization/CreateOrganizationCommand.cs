using LabDesk.SeedWork.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Commands.CreateOrganization
{
    public record CreateOrganizationCommand(
    string Name,
    string Slug,
    string? Description = null) : ICommand<Guid>;
}
