using BuildingBlocks.Application;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using OrganizationManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Services.CreateService
{
    public sealed class CreateServiceHandler(IServiceRepository services, IOrganizationManagementUnitOfWork unitOfWork, IUserContext user) : IRequestHandler<CreateServiceCommand, Guid>
    {
        public async Task<Guid> Handle(CreateServiceCommand command, CancellationToken ct)
        {
            if (!await services.OrganizationExistsAsync(command.OrganizationId, ct)) 
                throw new KeyNotFoundException("Organization not found or archived.");
            if (!await services.IsOrganizationAdminAsync(command.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required to create services.");
            if (!await services.TeamBelongsToOrganizationAsync(command.ResponsibleTeamId, command.OrganizationId, ct)) 
                throw new InvalidOperationException("Responsible team must be active and belong to the service organization.");
            if (await services.ExistsByNameAsync(command.OrganizationId, command.Name.Trim(), ct)) 
                throw new InvalidOperationException("Service name already exists in this organization.");

            var entity = Service.Create(command.OrganizationId, command.ResponsibleTeamId, command.Name, command.Description, DateTime.UtcNow);
            await services.AddAsync(entity, ct);
            await unitOfWork.SaveChangesAsync(ct);
            return entity.Id;
        }
    }
}
