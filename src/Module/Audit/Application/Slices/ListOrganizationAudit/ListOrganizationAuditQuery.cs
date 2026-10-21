using Audit.Application.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Slices.ListOrganizationAudit
{
    public sealed record ListOrganizationAuditQuery(Guid OrganizationId, int Page = 1, int PageSize = 50) : IRequest<IReadOnlyList<AuditEntryItem>>;

}
