using LabDesk.Modules.Identity.Application.DTOs;
using LabDesk.SeedWork.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.Modules.Identity.Application.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto?>;
}
