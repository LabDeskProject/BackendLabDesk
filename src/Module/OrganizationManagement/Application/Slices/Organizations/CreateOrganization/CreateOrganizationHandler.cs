using BuildingBlocks.Application;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using OrganizationManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Organizations.CreateOrganization
{
    public sealed class CreateOrganizationHandler(IOrganizationRepository organizations, IUserContext user, IOrganizationManagementUnitOfWork unitOfWork) : IRequestHandler<CreateOrganizationCommand, CreateOrganizationResult>
    {
        public async Task<CreateOrganizationResult> Handle(CreateOrganizationCommand command, CancellationToken ct)
        {
            if (await organizations.ExistsBySlugAsync(command.Slug, ct)) 
                throw new InvalidOperationException("Organization slug already exists.");

            var now = DateTime.UtcNow;
            var organization = Organization.Create(command.Name, command.Slug, command.Description, now);
            await organizations.AddAsync(organization, ct);
            await organizations.AddMemberAsync(new OrganizationUser(organization.Id, user.UserId, OrganizationRole.OrganizationAdmin, now), ct);
            await unitOfWork.SaveChangesAsync(ct);

            return new(organization.Id, organization.Name, organization.Slug, "Organization created successfully.");
        }
    }
}
