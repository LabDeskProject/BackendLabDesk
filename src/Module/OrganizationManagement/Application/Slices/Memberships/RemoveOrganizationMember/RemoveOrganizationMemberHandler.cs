using BuildingBlocks.Application;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using OrganizationManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Memberships.RemoveOrganizationMember
{
    public sealed class RemoveOrganizationMemberHandler(IOrganizationRepository organizations, IOrganizationUserRepository members, IUserContext user, IOrganizationManagementUnitOfWork unit) : IRequestHandler<RemoveOrganizationMemberCommand>
    {
        public async Task Handle(RemoveOrganizationMemberCommand command, CancellationToken ct)
        {
            if (!await members.IsAdminAsync(command.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required.");
            if (command.UserId == user.UserId) 
                throw new InvalidOperationException("An admin cannot remove their own membership.");
            var member = await organizations.GetMemberAsync(command.OrganizationId, command.UserId, ct) ?? 
                throw new KeyNotFoundException("Active membership not found.");

            var active = await organizations.ListMemberEntitiesAsync(command.OrganizationId, ct);
            if (member.Role == OrganizationRole.OrganizationAdmin && active.Count(x => x.Role == OrganizationRole.OrganizationAdmin) <= 1) throw new InvalidOperationException("The last organization admin cannot be removed.");
            member.Remove(DateTime.UtcNow);
            await unit.SaveChangesAsync(ct);
        }
    }
}
