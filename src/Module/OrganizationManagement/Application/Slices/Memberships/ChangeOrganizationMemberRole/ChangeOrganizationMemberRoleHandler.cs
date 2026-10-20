using BuildingBlocks.Application;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using OrganizationManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Memberships.ChangeOrganizationMemberRole
{
    public sealed class ChangeOrganizationMemberRoleHandler(IOrganizationRepository organizations, IOrganizationUserRepository members, IUserContext user, IOrganizationManagementUnitOfWork unit) : IRequestHandler<ChangeOrganizationMemberRoleCommand>
    {
        public async Task Handle(ChangeOrganizationMemberRoleCommand command, CancellationToken ct)
        {
            if (!await members.IsAdminAsync(command.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required.");
            var member = await organizations.GetMemberAsync(command.OrganizationId, command.UserId, ct) ?? 
                throw new KeyNotFoundException("Active membership not found.");
            if (member.Role == OrganizationRole.OrganizationAdmin && command.Role != OrganizationRole.OrganizationAdmin)
            {
                var active = await organizations.ListMemberEntitiesAsync(command.OrganizationId, ct);
                if (active.Count(x => x.Role == OrganizationRole.OrganizationAdmin) <= 1) 
                    throw new InvalidOperationException("The last organization admin cannot be demoted.");
            }
            member.ChangeRole(command.Role);
            await unit.SaveChangesAsync(ct);
        }
    }
}
