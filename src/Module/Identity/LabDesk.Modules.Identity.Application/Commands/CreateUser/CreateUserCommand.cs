using LabDesk.Modules.Identity.Domain.Aggregates;
using LabDesk.SeedWork.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LabDesk.Modules.Identity.Application.Commands.CreateUser
{
    public record CreateUserCommand(
         Guid OrganizationId,
         string Email,
         string FullName,
         Role Role,
         Guid? TeamId = null) : ICommand<Guid>;
}
