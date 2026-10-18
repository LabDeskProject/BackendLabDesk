using ApplicationCore.Interfaces;
using BuildingBlocks.Application;
using BuildingBlocks.Application.CQRS;
using DomainCore.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Organizations.CreateOrganization
{
    public sealed class CreateOrganizationCommandHandler
    : ICommandHandler<
        CreateOrganizationCommand,
        CreateOrganizationResult>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUnitOfWorkIdentityAccess _unitOfWork;

        private readonly IUserContext _userContext;

        public CreateOrganizationCommandHandler(
            IOrganizationRepository organizationRepository,
            IUserContext userContext,
            IUnitOfWorkIdentityAccess unitOfWork)
        {
            _organizationRepository = organizationRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateOrganizationResult> Handle(
            CreateOrganizationCommand command,
            CancellationToken cancellationToken)
        {
            var exists = await _organizationRepository
                .ExistsBySlugAsync(
                    command.Slug,
                    cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException(
                    "Organization slug already exists.");
            }

            var organization = new Organization(
                Guid.NewGuid(),
                command.Name,
                command.Slug,
                command.Description,
                DateTime.UtcNow);

            var organizationAdmin =
                new OrganizationUser(
                    organization.Id,
                    _userContext.UserId,
                    OrganizationRole.OrganizationAdmin,
                    DateTime.UtcNow);

            await _organizationRepository.AddAsync(
                organization,
                cancellationToken);

            await _organizationRepository.AddMemberAsync(
                organizationAdmin,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new CreateOrganizationResult(
                organization.Id,
                organization.Name,
                organization.Slug,
                "Organization created successfully.");
        }
    }
}