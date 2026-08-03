using LabDesk.Modules.Identity.Domain.Aggregates;
using LabDesk.Modules.Identity.Domain.IRepository;
using LabDesk.SeedWork.Application.Interfaces;
using LabDesk.SeedWork.Application.Results;
using LabDesk.SeedWork.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUserRepository userRepository, IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId, cancellationToken);
            if (organization is null)
            {
                return Result<Guid>.Failure("Tổ chức không tồn tại.");
            }

            var emailExists = await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
            if (emailExists)
            {
                return Result<Guid>.Failure($"Email '{request.Email}' đã được sử dụng.");
            }

            var user = new User(
                id: Guid.NewGuid(),
                organizationId: request.OrganizationId,
                email: request.Email,
                fullName: request.FullName,
                role: request.Role,
                teamId: request.TeamId);

            _userRepository.Add(user);
            await _unitOfWork.SaveEntitiesAsync(cancellationToken);

            return Result<Guid>.Success(user.Id);
        }
    }
}
