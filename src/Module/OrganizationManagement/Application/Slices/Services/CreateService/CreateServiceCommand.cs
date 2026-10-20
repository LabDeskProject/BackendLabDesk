using BuildingBlocks.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Services.CreateService
{
    public sealed record CreateServiceCommand(Guid OrganizationId, Guid ResponsibleTeamId, string Name, string? Description) : ICommand<Guid>;
}
