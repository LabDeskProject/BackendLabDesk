using BuildingBlocks.Application;
using BuildingBlocks.Application.CQRS;
using DomainCore.Organizations;
using DomainCore.Teams;
using FluentValidation;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using OrganizationManagement.Domain;

namespace OrganizationManagement.Application.Slices;

public sealed record CreateOrganizationCommand(string Name, string Slug, string? Description) : ICommand<CreateOrganizationResult>;
public sealed record CreateOrganizationResult(Guid Id, string Name, string Slug, string Message);
public sealed class CreateOrganizationValidator : AbstractValidator<CreateOrganizationCommand>
{
    public CreateOrganizationValidator() { RuleFor(x => x.Name).NotEmpty().MaximumLength(200); RuleFor(x => x.Slug).NotEmpty().MaximumLength(100); RuleFor(x => x.Description).MaximumLength(1000); }
}
public sealed class CreateOrganizationHandler(IOrganizationRepository organizations, IUserContext user, IOrganizationManagementUnitOfWork unitOfWork) : IRequestHandler<CreateOrganizationCommand, CreateOrganizationResult>
{
    public async Task<CreateOrganizationResult> Handle(CreateOrganizationCommand command, CancellationToken ct)
    {
        if (await organizations.ExistsBySlugAsync(command.Slug, ct)) throw new InvalidOperationException("Organization slug already exists.");
        var now = DateTime.UtcNow; var organization = Organization.Create(command.Name, command.Slug, command.Description, now);
        await organizations.AddAsync(organization, ct); await organizations.AddMemberAsync(new OrganizationUser(organization.Id, user.UserId, OrganizationRole.OrganizationAdmin, now), ct); await unitOfWork.SaveChangesAsync(ct);
        return new(organization.Id, organization.Name, organization.Slug, "Organization created successfully.");
    }
}

public sealed record CreateTeamCommand(Guid OrganizationId, string Name, string? Description) : ICommand<CreateTeamResult>;
public sealed record CreateTeamResult(Guid Id, Guid OrganizationId, string Name);
public sealed class CreateTeamValidator : AbstractValidator<CreateTeamCommand> { public CreateTeamValidator() { RuleFor(x => x.OrganizationId).NotEmpty(); RuleFor(x => x.Name).NotEmpty().MaximumLength(150); RuleFor(x => x.Description).MaximumLength(500); } }
public sealed class CreateTeamHandler(ITeamRepository teams, IOrganizationRepository organizations, IOrganizationUserRepository members, IUserContext user, IOrganizationManagementUnitOfWork unitOfWork) : IRequestHandler<CreateTeamCommand, CreateTeamResult>
{
    public async Task<CreateTeamResult> Handle(CreateTeamCommand command, CancellationToken ct)
    {
        if (!await organizations.ExistsByIdAsync(command.OrganizationId, ct)) throw new KeyNotFoundException("Organization not found.");
        if (!await members.ExistsAsync(command.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("User does not belong to this organization.");
        if (!await members.IsAdminAsync(command.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("Organization admin role is required to create teams.");
        if (await teams.ExistsByNameAsync(command.OrganizationId, command.Name, ct)) throw new InvalidOperationException("A team with the same name already exists.");
        var team = Team.Create(command.OrganizationId, command.Name, command.Description, DateTime.UtcNow); await teams.AddAsync(team, ct); await unitOfWork.SaveChangesAsync(ct); return new(team.Id, team.OrganizationId, team.Name);
    }
}

public sealed record ListMyOrganizationsQuery : IRequest<IReadOnlyList<OrganizationListItem>>;
public sealed class ListMyOrganizationsHandler(IOrganizationRepository organizations, IUserContext user) : IRequestHandler<ListMyOrganizationsQuery, IReadOnlyList<OrganizationListItem>>
{
    public Task<IReadOnlyList<OrganizationListItem>> Handle(ListMyOrganizationsQuery query, CancellationToken ct) => organizations.ListForUserAsync(user.UserId, ct);
}
public sealed record ListOrganizationMembersQuery(Guid OrganizationId) : IRequest<IReadOnlyList<OrganizationMemberItem>>;
public sealed class ListOrganizationMembersHandler(IOrganizationRepository organizations, IUserContext user) : IRequestHandler<ListOrganizationMembersQuery, IReadOnlyList<OrganizationMemberItem>>
{
    public async Task<IReadOnlyList<OrganizationMemberItem>> Handle(ListOrganizationMembersQuery query, CancellationToken ct)
    {
        if (!await organizations.ExistsByIdAsync(query.OrganizationId, ct)) throw new KeyNotFoundException("Organization not found.");
        if (!await organizations.IsAdminAsync(query.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("Organization admin role is required to list members.");
        return await organizations.ListMembersAsync(query.OrganizationId, ct);
    }
}
