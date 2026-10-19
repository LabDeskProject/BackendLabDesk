using DomainCore.Organizations;
using FluentValidation;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;
using BuildingBlocks.Application;
namespace OrganizationManagement.Application.Slices;
public sealed record AddOrganizationMemberCommand(Guid OrganizationId, Guid UserId, OrganizationRole Role) : IRequest;
public sealed class AddOrganizationMemberValidator : AbstractValidator<AddOrganizationMemberCommand>
{
    public AddOrganizationMemberValidator() { RuleFor(x => x.OrganizationId).NotEmpty(); RuleFor(x => x.UserId).NotEmpty(); RuleFor(x => x.Role).IsInEnum(); }
}
public sealed class AddOrganizationMemberHandler(IOrganizationRepository organizations, IOrganizationUserRepository members, IUserContext user, IOrganizationManagementUnitOfWork unit) : IRequestHandler<AddOrganizationMemberCommand>
{
    public async Task Handle(AddOrganizationMemberCommand command, CancellationToken ct)
    {
        if (!await organizations.ExistsByIdAsync(command.OrganizationId, ct)) throw new KeyNotFoundException("Organization not found or archived.");
        if (!await members.IsAdminAsync(command.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("Organization admin role is required.");
        var existing = await organizations.GetAnyMemberAsync(command.OrganizationId, command.UserId, ct);
        if (existing is not null)
        {
            if (existing.IsActive) throw new InvalidOperationException("User is already an active member.");
            existing.Reactivate(command.Role, DateTime.UtcNow);
        }
        else await organizations.AddMemberAsync(new OrganizationUser(command.OrganizationId, command.UserId, command.Role, DateTime.UtcNow), ct);
        await unit.SaveChangesAsync(ct);
    }
}
public sealed record RemoveOrganizationMemberCommand(Guid OrganizationId, Guid UserId) : IRequest;
public sealed class RemoveOrganizationMemberHandler(IOrganizationRepository organizations, IOrganizationUserRepository members, IUserContext user, IOrganizationManagementUnitOfWork unit) : IRequestHandler<RemoveOrganizationMemberCommand>
{
    public async Task Handle(RemoveOrganizationMemberCommand command, CancellationToken ct)
    {
        if (!await members.IsAdminAsync(command.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("Organization admin role is required.");
        if (command.UserId == user.UserId) throw new InvalidOperationException("An admin cannot remove their own membership.");
        var member = await organizations.GetMemberAsync(command.OrganizationId, command.UserId, ct) ?? throw new KeyNotFoundException("Active membership not found.");
        var active = await organizations.ListMemberEntitiesAsync(command.OrganizationId, ct);
        if (member.Role == OrganizationRole.OrganizationAdmin && active.Count(x => x.Role == OrganizationRole.OrganizationAdmin) <= 1) throw new InvalidOperationException("The last organization admin cannot be removed.");
        member.Remove(DateTime.UtcNow); await unit.SaveChangesAsync(ct);
    }
}
public sealed record ChangeOrganizationMemberRoleCommand(Guid OrganizationId, Guid UserId, OrganizationRole Role) : IRequest;
public sealed class ChangeOrganizationMemberRoleHandler(IOrganizationRepository organizations, IOrganizationUserRepository members, IUserContext user, IOrganizationManagementUnitOfWork unit) : IRequestHandler<ChangeOrganizationMemberRoleCommand>
{
    public async Task Handle(ChangeOrganizationMemberRoleCommand command, CancellationToken ct)
    {
        if (!await members.IsAdminAsync(command.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("Organization admin role is required.");
        var member = await organizations.GetMemberAsync(command.OrganizationId, command.UserId, ct) ?? throw new KeyNotFoundException("Active membership not found.");
        if (member.Role == OrganizationRole.OrganizationAdmin && command.Role != OrganizationRole.OrganizationAdmin)
        {
            var active = await organizations.ListMemberEntitiesAsync(command.OrganizationId, ct);
            if (active.Count(x => x.Role == OrganizationRole.OrganizationAdmin) <= 1) throw new InvalidOperationException("The last organization admin cannot be demoted.");
        }
        member.ChangeRole(command.Role); await unit.SaveChangesAsync(ct);
    }
}
