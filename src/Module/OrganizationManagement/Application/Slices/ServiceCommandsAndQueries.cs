using BuildingBlocks.Application.CQRS;
using FluentValidation;
using MediatR;
using OrganizationManagement.Application.Interfaces;
using BuildingBlocks.Application;
using OrganizationManagement.Domain;

namespace OrganizationManagement.Application.Slices;

public sealed record CreateServiceCommand(Guid OrganizationId, Guid ResponsibleTeamId, string Name, string? Description) : ICommand<Guid>;
public sealed class CreateServiceValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceValidator() { RuleFor(x => x.OrganizationId).NotEmpty(); RuleFor(x => x.ResponsibleTeamId).NotEmpty(); RuleFor(x => x.Name).NotEmpty().MaximumLength(150); RuleFor(x => x.Description).MaximumLength(1000); }
}
public sealed class CreateServiceHandler(IServiceRepository services, IOrganizationManagementUnitOfWork unitOfWork, IUserContext user) : IRequestHandler<CreateServiceCommand, Guid>
{
    public async Task<Guid> Handle(CreateServiceCommand command, CancellationToken ct)
    {
        if (!await services.OrganizationExistsAsync(command.OrganizationId, ct)) throw new KeyNotFoundException("Organization not found or archived.");
        if (!await services.IsOrganizationAdminAsync(command.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("Organization admin role is required to create services.");
        if (!await services.TeamBelongsToOrganizationAsync(command.ResponsibleTeamId, command.OrganizationId, ct)) throw new InvalidOperationException("Responsible team must be active and belong to the service organization.");
        if (await services.ExistsByNameAsync(command.OrganizationId, command.Name.Trim(), ct)) throw new InvalidOperationException("Service name already exists in this organization.");
        var entity = DomainCore.Service.Service.Create(command.OrganizationId, command.ResponsibleTeamId, command.Name, command.Description, DateTime.UtcNow);
        await services.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return entity.Id;
    }
}
public sealed record GetServiceQuery(Guid ServiceId) : IQuery<ServiceListItem?>;
public sealed class GetServiceHandler(IServiceQueries queries, IServiceRepository services, IUserContext user) : IRequestHandler<GetServiceQuery, ServiceListItem?>
{
    public async Task<ServiceListItem?> Handle(GetServiceQuery request, CancellationToken ct)
    {
        var result = await queries.GetDetailsAsync(request.ServiceId, ct);
        if (result is null) return null;
        if (!await services.IsOrganizationMemberAsync(result.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("User does not belong to this organization.");
        return result;
    }
}
public sealed record ListOrganizationServicesQuery(Guid OrganizationId) : IQuery<IReadOnlyList<ServiceListItem>>;
public sealed class ListOrganizationServicesHandler(IServiceQueries queries, IServiceRepository services, IUserContext user) : IRequestHandler<ListOrganizationServicesQuery, IReadOnlyList<ServiceListItem>>
{
    public async Task<IReadOnlyList<ServiceListItem>> Handle(ListOrganizationServicesQuery request, CancellationToken ct)
    {
        if (!await services.IsOrganizationMemberAsync(request.OrganizationId, user.UserId, ct)) throw new UnauthorizedAccessException("User does not belong to this organization.");
        return await queries.ListByOrganizationAsync(request.OrganizationId, ct);
    }
}
