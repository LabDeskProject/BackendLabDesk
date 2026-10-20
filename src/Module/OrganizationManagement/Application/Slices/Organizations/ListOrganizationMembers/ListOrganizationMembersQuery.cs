using MediatR;
using OrganizationManagement.Application.Interfaces;

namespace OrganizationManagement.Application.Slices.Organizations.ListOrganizationMembers;

public sealed record ListOrganizationMembersQuery(Guid OrganizationId) : IRequest<IReadOnlyList<OrganizationMemberItem>>;
