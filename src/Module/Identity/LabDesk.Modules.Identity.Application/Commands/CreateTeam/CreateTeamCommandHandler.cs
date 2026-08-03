using LabDesk.Modules.Identity.Domain.Aggregates;
using LabDesk.Modules.Identity.Domain.IRepository;
using LabDesk.SeedWork.Application.Interfaces;
using LabDesk.SeedWork.Application.Results;
using LabDesk.SeedWork.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Commands.CreateTeam
{
    public class CreateTeamCommandHandler : ICommandHandler<CreateTeamCommand, Guid>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeamCommandHandler(ITeamRepository teamRepository, IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _organizationRepository = organizationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId, cancellationToken);
            if (organization is null)
            {
                return Result<Guid>.Failure("Tổ chức không tồn tại.");
            }

            var team = new Team(
                id: Guid.NewGuid(),
                organizationId: request.OrganizationId,
                name: request.Name,
                description: request.Description,
                teamLeadId: request.TeamLeadId);

            _teamRepository.Add(team);
            await _unitOfWork.SaveEntitiesAsync(cancellationToken);

            return Result<Guid>.Success(team.Id);
        }
    }
}
