using BuildingBlocks.Application;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using OrganizationManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationManagement.Application.Slices.Teams.CreateTeam
{
    public sealed class CreateTeamHandler(ITeamRepository teams, IOrganizationRepository organizations, IOrganizationUserRepository members, IUserContext user, IOrganizationManagementUnitOfWork unitOfWork) : IRequestHandler<CreateTeamCommand, CreateTeamResult>
    {
        public async Task<CreateTeamResult> Handle(CreateTeamCommand command, CancellationToken ct)
        {
            if (!await organizations.ExistsByIdAsync(command.OrganizationId, ct)) 
                throw new KeyNotFoundException("Organization not found.");
            if (!await members.ExistsAsync(command.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("User does not belong to this organization.");
            if (!await members.IsAdminAsync(command.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required to create teams.");
            if (await teams.ExistsByNameAsync(command.OrganizationId, command.Name, ct)) 
                throw new InvalidOperationException("A team with the same name already exists.");

            var team = Team.Create(command.OrganizationId, command.Name, command.Description, DateTime.UtcNow);
            await teams.AddAsync(team, ct);
            await unitOfWork.SaveChangesAsync(ct);
            return new(team.Id, team.OrganizationId, team.Name);
        }
    }

}
