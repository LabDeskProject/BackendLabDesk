using LabDesk.Modules.Identity.Application.DTOs;
using LabDesk.Modules.Identity.Application.Interfaces;
using LabDesk.SeedWork.Application.CQRS;
using LabDesk.SeedWork.Application.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IIdentityDbContext _identityDbContext;
        public GetUserByIdQueryHandler(IIdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public async Task<Result<UserDto?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityDbContext.Users
            .AsNoTracking()
            .Where(x => x.Id == request.UserId)
            .Select(x => new UserDto(
                x.Id,
                x.OrganizationId,
                x.Email,
                x.FullName,
                x.Role.ToString(),
                x.TeamId,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

            return Result<UserDto?>.Success(user);
        }
    }
}
