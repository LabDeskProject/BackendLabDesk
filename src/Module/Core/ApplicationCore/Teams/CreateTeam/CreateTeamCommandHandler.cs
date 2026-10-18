using ApplicationCore.Interfaces;
using BuildingBlocks.Application;
using DomainCore.Teams;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Teams.CreateTeam
{
    public sealed class CreateTeamCommandHandler
    : IRequestHandler<CreateTeamCommand, CreateTeamResult>
    {
        private readonly ITeamRepository _teamRepository;

        private readonly IOrganizationRepository _organizationRepository;

        private readonly IOrganizationUserRepository _organizationUserRepository;

        private readonly IUnitOfWorkIdentityAccess _unitOfWork;

        private readonly IUserContext _userContext;

        public CreateTeamCommandHandler(
            ITeamRepository teamRepository,
            IOrganizationRepository organizationRepository,
            IOrganizationUserRepository organizationUserRepository,
            IUnitOfWorkIdentityAccess unitOfWork,
            IUserContext userContext)
        {
            _teamRepository = teamRepository;
            _organizationRepository = organizationRepository;
            _organizationUserRepository = organizationUserRepository;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<CreateTeamResult> Handle(
            CreateTeamCommand command,
            CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;

            var organizationExists =
                await _organizationRepository.ExistsByIdAsync(
                    command.OrganizationId,
                    cancellationToken);

            if (!organizationExists)
            {
                throw new InvalidOperationException(
                    "Organization not found.");
            }

            var isMember =
                await _organizationUserRepository.ExistsAsync(
                    command.OrganizationId,
                    userId,
                    cancellationToken);

            if (!isMember)
            {
                throw new UnauthorizedAccessException(
                    "User does not belong to this organization.");
            }

            var teamExists =
                await _teamRepository.ExistsByNameAsync(
                    command.OrganizationId,
                    command.Name,
                    cancellationToken);

            if (teamExists)
            {
                throw new InvalidOperationException(
                    "A team with the same name already exists.");
            }

            var team = Team.Create(
                command.OrganizationId,
                command.Name,
                command.Description);

            await _teamRepository.AddAsync(team, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateTeamResult(
                team.Id,
                team.OrganizationId,
                team.Name);
        }
    }
}
