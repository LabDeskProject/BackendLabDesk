using LabDesk.Modules.Identity.Domain.Aggregates;
using LabDesk.Modules.Identity.Domain.IRepository;
using LabDesk.SeedWork.Application.Interfaces;
using LabDesk.SeedWork.Application.Results;
using LabDesk.SeedWork.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Commands.CreateOrganization
{
    public class CreateOrganizationCommandHandler : ICommandHandler<CreateOrganizationCommand, Guid>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrganizationCommandHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
        {
            _organizationRepository = organizationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var slugExists = await _organizationRepository.ExistsBySlugAsync(request.Slug, cancellationToken);
            if (slugExists)
            {
                return  Result<Guid>.Failure($"Slug '{request.Slug}' đã tồn tại trong hệ thống.");
            }

            var organization = new Organization(
                id: Guid.NewGuid(),
                name: request.Name,
                slug: request.Slug,
                description: request.Description);

            _organizationRepository.Add(organization);
            await _unitOfWork.SaveEntitiesAsync(cancellationToken);

            return Result<Guid>.Success(organization.Id);
        }
    }

}
