using Audit.Application.Contracts;
using Audit.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Audit.Application.Slices.ListEntityAudit
{
    public sealed class ListEntityAuditHandler(IAuditQueries queries, IAuditAccess access, IAuditUserContext user) : IRequestHandler<ListEntityAuditQuery, IReadOnlyList<AuditEntryItem>>
    {
        public async Task<IReadOnlyList<AuditEntryItem>> Handle(ListEntityAuditQuery q, CancellationToken ct) 
        { 
            if (!user.IsAdmin && !await access.IsOrganizationAdminAsync(q.OrganizationId, user.UserId, ct)) 
                throw new UnauthorizedAccessException("Organization admin role is required to view audit entries."); 
            return await queries.ListForEntityAsync(q.OrganizationId, q.EntityType, q.EntityId, (q.Page - 1) * q.PageSize, q.PageSize, ct); 
        }
    }
}
