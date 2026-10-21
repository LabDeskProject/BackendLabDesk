using Audit.Application.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Slices.ListEntityAudit
{
    public sealed record ListEntityAuditQuery(Guid OrganizationId, string EntityType, string EntityId, int Page = 1, int PageSize = 50) : IRequest<IReadOnlyList<AuditEntryItem>>;

}
