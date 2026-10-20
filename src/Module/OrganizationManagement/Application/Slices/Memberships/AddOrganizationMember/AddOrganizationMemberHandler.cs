using BuildingBlocks.Application;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using OrganizationManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Memberships.AddOrganizationMember
{
    public sealed class AddOrganizationMemberHandler(IOrganizationRepository organizations, IOrganizationUserRepository members, IUserContext user, IOrganizationManagementUnitOfWork unit) : IRequestHandler<AddOrganizationMemberCommand>
    {
        public async Task Handle(AddOrganizationMemberCommand command, CancellationToken ct)
        {
            if (!await organizations.ExistsByIdAsync(command.OrganizationId, ct)) 
                throw new KeyNotFoundException("Organization not found or archived.");
            if (!await members.IsAdminAsync(command.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required.");

            var existing = await organizations.GetAnyMemberAsync(command.OrganizationId, command.UserId, ct);
            if (existing is not null)
            {
                if (existing.IsActive) 
                    throw new InvalidOperationException("User is already an active member.");
                existing.Reactivate(command.Role, DateTime.UtcNow);
            }
            else await organizations.AddMemberAsync(new OrganizationUser(command.OrganizationId, command.UserId, command.Role, DateTime.UtcNow), ct);
            await unit.SaveChangesAsync(ct);
        }
    }
}
