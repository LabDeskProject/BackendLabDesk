using BuildingBlocks.Application.CQRS;
using OrganizationManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Services.GetService
{
    public sealed record GetServiceQuery(Guid ServiceId) : IQuery<ServiceListItem?>;
}
